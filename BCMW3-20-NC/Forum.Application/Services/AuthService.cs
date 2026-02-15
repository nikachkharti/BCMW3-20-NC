using Forum.API.Application.DTO.Auth;
using Forum.Application.Contracts.Repository;
using Forum.Application.Contracts.Service;
using Forum.Application.Exceptions;
using Forum.Domain.Entities;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;

namespace Forum.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;
        private const string _adminRoleName = "Admin";
        private const string _customerRoleName = "Customer";

        public AuthService(
            IUserRepository userRepository,
            IJwtTokenGenerator jwtTokenGenerator,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mapper = mapper;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginRequestDto.UserName);

            if (user == null)
                throw new BadRequestException($"[Login Failure] User with username: {loginRequestDto.UserName} not found.");

            if (user.LockoutEnabled && !user.EmailConfirmed)
                throw new BadRequestException($"[Login Failure] User account is inactive");

            bool isPasswordValid = await _userRepository.IsPasswordValidAsync(user, loginRequestDto.Password);

            if (!isPasswordValid)
                throw new BadRequestException($"[Login Failure] Incorrect password");


            var roles = await _userRepository.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user, roles);

            return new LoginResponseDto()
            {
                Token = token,
            };
        }
        public Task<string> Register(RegistrationRequestDto dto) => RegisterInternal(dto, _customerRoleName, "Customer");
        public Task<string> RegisterAdmin(RegistrationRequestDto dto) => RegisterInternal(dto, _adminRoleName, "Admin");
        public async Task<bool> TryUnlockUserAccount(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new BadRequestException($"[Account Unlock Failure] {userId} is invalid");

            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                throw new NotFoundException($"[Account Unlock Failure] User with id: {userId} not found.");

            if (user.LockoutEnabled)
            {
                var result = await _userRepository.UnlockUserAccount(user);
                return result is not null && result.Succeeded;
            }

            return false;
        }
        public async Task<bool> TryLockUserAccount(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new BadRequestException($"[Account Lock Failure] {userId} is invalid");

            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                throw new NotFoundException($"[Account Lock Failure] User with id: {userId} not found.");

            if (!user.LockoutEnabled)
            {
                var result = await _userRepository.LockUserAccount(user);
                return result is not null && result.Succeeded;
            }

            return false;
        }




        #region HELPERS
        private async Task<string> RegisterInternal(RegistrationRequestDto dto, string roleName, string userType)
        {
            var user = _mapper.Map<ApplicationUser>(dto);

            var result = await _userRepository.RegisterAsync(user, dto.Password);

            if (!result.Succeeded)
                throw new BadRequestException(
                    $"[{userType} Registration Failure] Unable to register {userType.ToLower()} with email: {dto.Email}");
            try
            {
                var userToReturn = await _userRepository.GetByEmailAsync(dto.Email.ToLower());

                if (userToReturn == null)
                    throw new NotFoundException(
                        $"[{userType} Registration Failure] {userType} not found with email: {dto.Email}");

                await EnsureRoleExists(roleName);
                await _userRepository.AddToRoleAsync(userToReturn, roleName);

                //Lock account by default while registration if no admin
                await (roleName != _adminRoleName
                    ? _userRepository.LockUserAccount(user)
                    : _userRepository.UnlockUserAccount(user));

                return userToReturn.Id;
            }
            catch (NotFoundException)
            {
                throw;
            }
        }
        private async Task EnsureRoleExists(string roleName)
        {
            if (!await _userRepository.RoleExistsAsync(roleName))
                await _userRepository.CreateRoleAsync(new IdentityRole(roleName));
        }
        #endregion


    }
}
