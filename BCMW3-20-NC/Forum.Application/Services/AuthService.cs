using Forum.API.Application.DTO.Auth;
using Forum.Application.Contracts.Service;
using Forum.Application.Exceptions;
using Forum.Domain.Entities;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Forum.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;
        private const string _adminRoleName = "Admin";
        private const string _customerRoleName = "Customer";

        public AuthService(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IJwtTokenGenerator jwtTokenGenerator,
            IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mapper = mapper;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.UserName.ToLower() == loginRequestDto.UserName.ToLower());

            if (user == null)
                throw new BadRequestException($"[Login Failure] User with username: {loginRequestDto.UserName} not found.");

            bool isPasswordValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (!isPasswordValid)
                throw new BadRequestException($"[Login Failure] Incorrect password");

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user, roles);

            return new LoginResponseDto()
            {
                Token = token,
            };
        }
        public Task<string> Register(RegistrationRequestDto dto) => RegisterInternal(dto, _customerRoleName, "Customer");
        public Task<string> RegisterAdmin(RegistrationRequestDto dto) => RegisterInternal(dto, _adminRoleName, "Admin");



        #region HELPERS
        private async Task<string> RegisterInternal(RegistrationRequestDto dto, string roleName, string userType)
        {
            var user = _mapper.Map<ApplicationUser>(dto);

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                throw new BadRequestException(
                    $"[{userType} Registration Failure] Unable to register {userType.ToLower()} with email: {dto.Email}");
            try
            {
                var userToReturn = await _context.ApplicationUsers
                    .FirstOrDefaultAsync(x => x.Email.ToLower() == dto.Email.ToLower());

                if (userToReturn == null)
                    throw new NotFoundException(
                        $"[{userType} Registration Failure] {userType} not found with email: {dto.Email}");

                await EnsureRoleExists(roleName);
                await _userManager.AddToRoleAsync(userToReturn, roleName);

                return userToReturn.Id;
            }
            catch (NotFoundException)
            {
                throw;
            }
        }
        private async Task EnsureRoleExists(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
                await _roleManager.CreateAsync(new IdentityRole(roleName));
        }
        #endregion


    }
}
