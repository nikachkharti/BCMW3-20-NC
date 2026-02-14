using Forum.Application.Contracts.Repository;
using Forum.Domain.Entities;
using Forum.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Forum.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRepository(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string roleName)
        {
            return await _userManager.AddToRoleAsync(user, roleName);
        }
        public async Task<IdentityResult> RegisterAsync(ApplicationUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }
        public async Task<ApplicationUser> GetByEmailAsync(string email)
        {
            return await _context.ApplicationUsers
                .FirstOrDefaultAsync(x => x.UserName.ToLower().Trim() == email.ToLower().Trim());
        }
        public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }
        public async Task<bool> IsPasswordValidAsync(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }
        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }
        public async Task<IdentityResult> CreateRoleAsync(IdentityRole role)
        {
            return await _roleManager.CreateAsync(role);
        }
        public async Task<IdentityResult> LockUserAccount(ApplicationUser user)
        {
            return await _userManager.SetLockoutEnabledAsync(user, enabled: true);
        }
        public async Task<IdentityResult> UnlockUserAccount(ApplicationUser user)
        {
            return await _userManager.SetLockoutEnabledAsync(user, enabled: false);
        }
        public async Task<ApplicationUser> GetByIdAsync(string userId)
        {
            return await _context.ApplicationUsers
                .FirstOrDefaultAsync(x => x.Id.ToLower().Trim() == userId.ToLower().Trim());
        }
    }
}
