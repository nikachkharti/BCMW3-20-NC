using Forum.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Forum.Application.Features.Topics.Common
{
    public class TopicHelper(IHttpContextAccessor httpContextAccessor)
    {
        public bool UserCanModifyContent(Topic content)
        {
            string authenticatedUserId = AuthenticatedUserId();

            if (string.IsNullOrWhiteSpace(authenticatedUserId))
                return false;

            if (content.AuthorId.Trim() != authenticatedUserId.Trim())
                return false;

            return true;
        }
        public string AuthenticatedUserId() =>
            httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true
                ? httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
                : string.Empty;
    }
}
