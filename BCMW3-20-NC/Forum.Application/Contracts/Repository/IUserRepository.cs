using Forum.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Forum.Application.Contracts.Repository
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetByEmailAsync(string email);
        Task<ApplicationUser> GetByIdAsync(string id);
        Task<List<ApplicationUser>> GetUnlockedUsers();

        Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
        Task AddToRoleAsync(ApplicationUser user, string role);

        Task<IList<string>> GetRolesAsync(ApplicationUser user);
        Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user);

        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<bool> IsLockedOutAsync(ApplicationUser user);
        Task<bool> IsEmailConfirmedAsync(ApplicationUser user);

        Task AccessFailedAsync(ApplicationUser user);
        Task ResetAccessFailedCountAsync(ApplicationUser user);
        Task LockAsync(ApplicationUser user);
        Task UnlockAsync(ApplicationUser user);
        Task ClearLockoutAsync(ApplicationUser user);
        Task EnsureRoleExistsAsync(string roleName);
        Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token);
    }

}
