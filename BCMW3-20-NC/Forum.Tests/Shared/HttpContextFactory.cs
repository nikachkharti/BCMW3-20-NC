using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace Forum.Tests.Shared
{
    internal static class HttpContextFactory
    {
        /// <summary>Creates an IHttpContextAccessor whose user is authenticated with the given userId.</summary>
        public static IHttpContextAccessor Authenticated(string userId)
        {
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId) };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext { User = principal };
            var accessor = new Mock<IHttpContextAccessor>();
            accessor.Setup(a => a.HttpContext).Returns(httpContext);
            return accessor.Object;
        }

        /// <summary>Creates an IHttpContextAccessor that represents an unauthenticated request.</summary>
        public static IHttpContextAccessor Unauthenticated()
        {
            var httpContext = new DefaultHttpContext();
            var accessor = new Mock<IHttpContextAccessor>();
            accessor.Setup(a => a.HttpContext).Returns(httpContext);
            return accessor.Object;
        }
    }
}
