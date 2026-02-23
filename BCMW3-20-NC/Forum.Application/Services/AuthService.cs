using Forum.API.Application.DTO.Auth;
using Forum.Application.Contracts.Repository;
using Forum.Application.Contracts.Service;
using Forum.Application.Exceptions;
using Forum.Application.Validators;
using Forum.Domain.Entities;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Forum.Application.Services
{
    [Obsolete]
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _users;
        private readonly IJwtTokenGenerator _jwt;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private const string AdminRole = "Admin";
        private const string CustomerRole = "Customer";

        public AuthService(
            IUserRepository users,
            IJwtTokenGenerator jwt,
            INotificationService notificationService,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _users = users;
            _jwt = jwt;
            _notificationService = notificationService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }


        // ---------------- LOGIN ----------------
        public async Task<LoginResponseDto> Login(LoginRequestDto dto)
        {
            var user = await _users.GetByEmailAsync(dto.UserName)
                ?? throw new BadRequestException(Error.BuildErrorMessage("Login", "Invalid credentials"));

            var roles = await _users.GetRolesAsync(user);

            if (await _users.IsLockedOutAsync(user))
                throw new BadRequestException(Error.BuildErrorMessage("Login", "Account is locked"));

            if (!await _users.IsEmailConfirmedAsync(user) && !roles.Contains("Admin"))
                throw new BadRequestException(Error.BuildErrorMessage("Login", "Email is not confirmed"));

            if (!await _users.CheckPasswordAsync(user, dto.Password))
            {
                await _users.AccessFailedAsync(user);
                throw new BadRequestException(Error.BuildErrorMessage("Login", "Invalid credentials"));
            }

            await _users.ResetAccessFailedCountAsync(user);

            if (user.LockoutEnd != null)
                await _users.ClearLockoutAsync(user);


            var token = _jwt.GenerateToken(user, roles);

            return new LoginResponseDto { Token = token };
        }

        // ---------------- REGISTRATION ----------------
        public async Task<string> Register(RegistrationRequestDto dto)
        {
            var registeredUser = await RegisterInternal(dto, CustomerRole, lockByDefault: true);

            if (!string.IsNullOrEmpty(registeredUser.Id))
                await TrySendActivationEmailAsync(dto, registeredUser);

            return registeredUser.Id;
        }
        public async Task<string> RegisterAdmin(RegistrationRequestDto dto)
        {
            var registeredUser = await RegisterInternal(dto, AdminRole, lockByDefault: false);
            return registeredUser.Id;
        }


        // ---------------- ADMIN OPERATIONS ----------------
        public async Task<bool> TryUnlockUserAccount(string userId)
        {
            var user = await _users.GetByIdAsync(userId)
                ?? throw new NotFoundException(Error.BuildErrorMessage("TryUnlockUserAccount", "User not found"));

            await _users.UnlockAsync(user);
            return true;
        }
        public async Task<bool> TryUnlockUserAccount(ApplicationUser user)
        {
            await _users.UnlockAsync(user);
            return true;
        }
        public async Task<bool> TryLockUserAccount(string userId)
        {
            var user = await _users.GetByIdAsync(userId)
                ?? throw new NotFoundException(Error.BuildErrorMessage("TryLockUserAccount", "User not found"));

            await _users.LockAsync(user);
            return true;
        }


        // ---------------- ACCOUNT ACTIVATION ----------------
        public async Task<bool> TryActivateUserAsync(string userId, string token)
        {
            var user = await GetUser(userId);
            bool userUnlocked = default;
            bool emailConfirmed = default;

            if (user.LockoutEnd is not null)
                userUnlocked = await TryUnlockUserAccount(user);

            if (!user.EmailConfirmed)
                emailConfirmed = await TryConfirmUserAsync(user, token);

            return userUnlocked && emailConfirmed;
        }





        private async Task<ApplicationUser> GetUser(string userId)
        {
            return await _users.GetByIdAsync(userId)
                ?? throw new NotFoundException(Error.BuildErrorMessage("GetUser", "User not found"));
        }
        private async Task<bool> TryConfirmUserAsync(ApplicationUser user, string token)
        {
            if (user is null)
                throw new BadRequestException(Error.BuildErrorMessage("TryConfirmUserAsync", "User object is required for account confirmation"));

            if (string.IsNullOrWhiteSpace(token))
                throw new BadRequestException(Error.BuildErrorMessage("TryConfirmUserAsync", "Activation token is required for account confirmation"));

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            var result = await _users.ConfirmEmailAsync(user, decodedToken);
            return result.Succeeded;
        }
        private async Task<string> BuildActivationUri(ApplicationUser user)
        {
            var request = _httpContextAccessor.HttpContext.Request;

            var token = await _users.GenerateEmailConfirmationTokenAsync(user);

            // Encode decodedToken properly for URL
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            return new UriBuilder
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Port = request.Host.Port ?? -1,
                Path = "/api/auth/activate",
                Query = $"userId={user.Id}&token={encodedToken}"
            }.ToString();
        }
        private async Task TrySendActivationEmailAsync(RegistrationRequestDto dto, ApplicationUser newUser)
        {
            try
            {
                var activationLink = await BuildActivationUri(newUser);

                var notificationResult = await _notificationService.SendAsync(dto.Email, "Account Activation", activationLink);
            }
            catch (Exception ex)
            {
                throw new InternalServerException(Error.BuildErrorMessage("TrySendActivationEmailAsync", $"User : {newUser.Id} registered successfully but account activation process via email failed : {ex.Message}"));
            }
        }
        private async Task<ApplicationUser> RegisterInternal(RegistrationRequestDto dto, string role, bool lockByDefault)
        {
            var user = _mapper.Map<ApplicationUser>(dto);
            user.UserName = dto.Email;

            var result = await _users.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                throw new BadRequestException(Error.BuildErrorMessage("RegisterInternal", "Registration failed."));

            await _users.EnsureRoleExistsAsync(role);
            await _users.AddToRoleAsync(user, role);

            if (lockByDefault)
                await _users.LockAsync(user);
            else
                await _users.UnlockAsync(user);

            return user;
        }
    }
}
