using Forum.Domain.Entities;

namespace Forum.Application.Contracts.Service
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles);
    }
}
