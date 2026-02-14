using Forum.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Forum.Application.Contracts.Repository
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetByEmailAsync(string email);
        Task<ApplicationUser> GetByIdAsync(string userId);
        Task<bool> IsPasswordValidAsync(ApplicationUser user, string password);
        Task<IList<string>> GetRolesAsync(ApplicationUser user);
        Task<IdentityResult> RegisterAsync(ApplicationUser user, string password);
        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string roleName);
        Task<bool> RoleExistsAsync(string roleName);
        Task<IdentityResult> CreateRoleAsync(IdentityRole role);
        Task<IdentityResult> LockUserAccount(ApplicationUser user);
        Task<IdentityResult> UnlockUserAccount(ApplicationUser user);
    }
}
