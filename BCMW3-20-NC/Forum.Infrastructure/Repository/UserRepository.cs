using Forum.Application.Contracts.Repository;
using Forum.Domain.Entities;
using Forum.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Forum.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRepository(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public Task<ApplicationUser> GetByEmailAsync(string email)
            => _userManager.FindByNameAsync(email);

        public Task<ApplicationUser> GetByIdAsync(string id)
            => _userManager.FindByIdAsync(id);

        public async Task<List<ApplicationUser>> GetUnlockedUsers()
        {
            var utcNow = DateTime.UtcNow;

            return await _userManager.Users
                .Where(u => u.LockoutEnd <= utcNow)
                .ToListAsync();
        }


        public Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
            => _userManager.CreateAsync(user, password);


        public Task AddToRoleAsync(ApplicationUser user, string role)
            => _userManager.AddToRoleAsync(user, role);

        public Task<IList<string>> GetRolesAsync(ApplicationUser user)
            => _userManager.GetRolesAsync(user);

        public Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
            => _userManager.CheckPasswordAsync(user, password);

        public Task<bool> IsLockedOutAsync(ApplicationUser user)
            => _userManager.IsLockedOutAsync(user);

        public Task AccessFailedAsync(ApplicationUser user)
            => _userManager.AccessFailedAsync(user);

        public Task ResetAccessFailedCountAsync(ApplicationUser user)
            => _userManager.ResetAccessFailedCountAsync(user);

        public Task LockAsync(ApplicationUser user)
            => _userManager.SetLockoutEndDateAsync(
                user,
                DateTimeOffset.UtcNow.AddYears(100));

        public Task UnlockAsync(ApplicationUser user)
            => _userManager.SetLockoutEndDateAsync(user, null);

        public async Task EnsureRoleExistsAsync(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
                await _roleManager.CreateAsync(new IdentityRole(roleName));
        }

        public async Task ClearLockoutAsync(ApplicationUser user)
        {
            await _userManager.SetLockoutEndDateAsync(user, null);
        }

    }
}
