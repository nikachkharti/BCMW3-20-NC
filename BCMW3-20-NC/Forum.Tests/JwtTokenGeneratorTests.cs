using Forum.Application.Services;
using Forum.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace Forum.Tests
{
    // =========================================================================
    // JwtTokenGenerator Tests
    // =========================================================================

    public class JwtTokenGeneratorTests
    {
        private JwtTokenGenerator CreateGenerator(string secret = "6E85223D-3EA0-4110-A34D-58DB97A20BF7-AF9C7293-CA46-4DE9-88E9-7F5800DA7F1A", string issuer = "test-issuer", string audience = "test-audience")
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                ["JwtOptions:Secret"] = secret,
                ["JwtOptions:Issuer"] = issuer,
                ["JwtOptions:Audience"] = audience
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            return new JwtTokenGenerator(configuration);
        }

        [Fact]
        public void GenerateToken_ValidUser_ReturnsNonEmptyToken()
        {
            var generator = CreateGenerator();
            var user = new ApplicationUser { Id = "u1", Email = "a@b.com", UserName = "a@b.com", FullName = "Alice" };

            var token = generator.GenerateToken(user, new[] { "Customer" });

            Assert.False(string.IsNullOrWhiteSpace(token));
        }

        [Fact]
        public void GenerateToken_ValidUser_TokenContainsThreeParts()
        {
            var generator = CreateGenerator();
            var user = new ApplicationUser { Id = "u1", Email = "a@b.com", UserName = "a@b.com", FullName = "Alice" };

            var token = generator.GenerateToken(user, Array.Empty<string>());
            var parts = token.Split('.');

            Assert.Equal(3, parts.Length); // header.payload.signature
        }

        [Fact]
        public void GenerateToken_MultipleRoles_IncludesAllRoleClaims()
        {
            var generator = CreateGenerator();
            var user = new ApplicationUser { Id = "u1", Email = "a@b.com", UserName = "a@b.com", FullName = "Alice" };

            var token = generator.GenerateToken(user, new[] { "Admin", "Customer" });

            // Decode payload to verify roles are present
            var payload = token.Split('.')[1];
            // Pad base64 if needed
            payload += new string('=', (4 - payload.Length % 4) % 4);
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(payload));

            Assert.Contains("Admin", json);
            Assert.Contains("Customer", json);
        }

        [Fact]
        public void GenerateToken_DifferentUsers_ProduceDifferentTokens()
        {
            var generator = CreateGenerator();
            var user1 = new ApplicationUser { Id = "u1", Email = "a@b.com", UserName = "a@b.com", FullName = "Alice" };
            var user2 = new ApplicationUser { Id = "u2", Email = "b@c.com", UserName = "b@c.com", FullName = "Bob" };

            var token1 = generator.GenerateToken(user1, Array.Empty<string>());
            var token2 = generator.GenerateToken(user2, Array.Empty<string>());

            Assert.NotEqual(token1, token2);
        }

        [Fact]
        public void GenerateToken_ContainsSubAndEmailClaims()
        {
            var generator = CreateGenerator();
            var user = new ApplicationUser { Id = "user-123", Email = "test@example.com", UserName = "test@example.com", FullName = "Test User" };

            var token = generator.GenerateToken(user, Array.Empty<string>());
            var payload = token.Split('.')[1];
            payload += new string('=', (4 - payload.Length % 4) % 4);
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(payload));

            Assert.Contains("user-123", json);
            Assert.Contains("test@example.com", json);
        }
    }

}
