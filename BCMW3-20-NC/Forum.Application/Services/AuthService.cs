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
        private readonly IUserRepository _users;
        private readonly IJwtTokenGenerator _jwt;
        private readonly IMapper _mapper;

        private const string AdminRole = "Admin";
        private const string CustomerRole = "Customer";

        public AuthService(
            IUserRepository users,
            IJwtTokenGenerator jwt,
            IMapper mapper)
        {
            _users = users;
            _jwt = jwt;
            _mapper = mapper;
        }

        // ---------------- LOGIN ----------------

        public async Task<LoginResponseDto> Login(LoginRequestDto dto)
        {
            var user = await _users.GetByEmailAsync(dto.UserName)
                ?? throw new BadRequestException("Invalid credentials.");

            if (await _users.IsLockedOutAsync(user))
                throw new BadRequestException("Account is locked.");

            if (!await _users.CheckPasswordAsync(user, dto.Password))
            {
                await _users.AccessFailedAsync(user);
                throw new BadRequestException("Invalid credentials.");
            }

            await _users.ResetAccessFailedCountAsync(user);
            await _users.ClearLockoutAsync(user);


            var roles = await _users.GetRolesAsync(user);
            var token = _jwt.GenerateToken(user, roles);

            return new LoginResponseDto { Token = token };
        }

        // ---------------- REGISTRATION ----------------

        public Task<string> Register(RegistrationRequestDto dto)
            => RegisterInternal(dto, CustomerRole, lockByDefault: true);

        public Task<string> RegisterAdmin(RegistrationRequestDto dto)
            => RegisterInternal(dto, AdminRole, lockByDefault: false);

        private async Task<string> RegisterInternal(
            RegistrationRequestDto dto,
            string role,
            bool lockByDefault)
        {
            var user = _mapper.Map<ApplicationUser>(dto);
            user.UserName = dto.Email;

            var result = await _users.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                throw new BadRequestException("Registration failed.");

            await _users.EnsureRoleExistsAsync(role);
            await _users.AddToRoleAsync(user, role);

            if (lockByDefault)
                await _users.LockAsync(user);
            else
                await _users.UnlockAsync(user);

            return user.Id;
        }

        // ---------------- ADMIN OPERATIONS ----------------

        public async Task<bool> TryUnlockUserAccount(string userId)
        {
            var user = await _users.GetByIdAsync(userId)
                ?? throw new NotFoundException("User not found.");

            await _users.UnlockAsync(user);
            return true;
        }

        public async Task<bool> TryLockUserAccount(string userId)
        {
            var user = await _users.GetByIdAsync(userId)
                ?? throw new NotFoundException("User not found.");

            await _users.LockAsync(user);
            return true;
        }
    }
}
