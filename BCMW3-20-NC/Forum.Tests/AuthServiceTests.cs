using CloudinaryDotNet;
using Forum.API.Application.DTO.Auth;
using Forum.Application.Contracts.Repository;
using Forum.Application.Contracts.Service;
using Forum.Application.Exceptions;
using Forum.Application.Models.Notification;
using Forum.Application.Services;
using Forum.Domain.Entities;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Forum.Tests
{
    // =========================================================================
    // AuthService Tests
    // =========================================================================

    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _users = new();
        private readonly Mock<IJwtTokenGenerator> _jwt = new();
        private readonly Mock<IMapper> _mapper = new();
        private readonly Mock<INotificationService> _notification = new();
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor = new();

        private AuthService CreateService() =>
            new(_users.Object, _jwt.Object, _notification.Object, _httpContextAccessor.Object, _mapper.Object);

        // ---- Login ----

        [Fact]
        public async Task Login_UserNotFound_ThrowsBadRequestException()
        {
            _users.Setup(u => u.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);

            var svc = CreateService();
            await Assert.ThrowsAsync<BadRequestException>(() =>
                svc.Login(new LoginRequestDto { UserName = "x@y.com", Password = "pass" }));
        }

        [Fact]
        public async Task Login_LockedAccount_ThrowsBadRequestException()
        {
            var user = new ApplicationUser { Id = "u1", Email = "x@y.com", UserName = "x@y.com" };
            _users.Setup(u => u.GetByEmailAsync("x@y.com")).ReturnsAsync(user);
            _users.Setup(u => u.GetRolesAsync(user)).ReturnsAsync(new List<string>());
            _users.Setup(u => u.IsLockedOutAsync(user)).ReturnsAsync(true);

            var svc = CreateService();
            await Assert.ThrowsAsync<BadRequestException>(() =>
                svc.Login(new LoginRequestDto { UserName = "x@y.com", Password = "pass" }));
        }

        [Fact]
        public async Task Login_EmailNotConfirmed_NonAdmin_ThrowsBadRequestException()
        {
            var user = new ApplicationUser { Id = "u1", Email = "x@y.com", UserName = "x@y.com" };
            _users.Setup(u => u.GetByEmailAsync("x@y.com")).ReturnsAsync(user);
            _users.Setup(u => u.GetRolesAsync(user)).ReturnsAsync(new List<string> { "Customer" });
            _users.Setup(u => u.IsLockedOutAsync(user)).ReturnsAsync(false);
            _users.Setup(u => u.IsEmailConfirmedAsync(user)).ReturnsAsync(false);

            var svc = CreateService();
            await Assert.ThrowsAsync<BadRequestException>(() =>
                svc.Login(new LoginRequestDto { UserName = "x@y.com", Password = "pass" }));
        }

        [Fact]
        public async Task Login_AdminEmailNotConfirmed_Allowed()
        {
            var user = new ApplicationUser { Id = "u1", Email = "admin@y.com", UserName = "admin@y.com" };
            _users.Setup(u => u.GetByEmailAsync("admin@y.com")).ReturnsAsync(user);
            _users.Setup(u => u.GetRolesAsync(user)).ReturnsAsync(new List<string> { "Admin" });
            _users.Setup(u => u.IsLockedOutAsync(user)).ReturnsAsync(false);
            _users.Setup(u => u.IsEmailConfirmedAsync(user)).ReturnsAsync(false);
            _users.Setup(u => u.CheckPasswordAsync(user, "pass")).ReturnsAsync(true);
            _users.Setup(u => u.ResetAccessFailedCountAsync(user)).Returns(Task.CompletedTask);
            _users.Setup(u => u.ClearLockoutAsync(user)).Returns(Task.CompletedTask);
            _jwt.Setup(j => j.GenerateToken(user, It.IsAny<IEnumerable<string>>())).Returns("token-123");

            var svc = CreateService();
            var result = await svc.Login(new LoginRequestDto { UserName = "admin@y.com", Password = "pass" });

            Assert.Equal("token-123", result.Token);
        }

        [Fact]
        public async Task Login_WrongPassword_IncreasesFailedCount_Throws()
        {
            var user = new ApplicationUser { Id = "u1", Email = "x@y.com", UserName = "x@y.com" };
            _users.Setup(u => u.GetByEmailAsync("x@y.com")).ReturnsAsync(user);
            _users.Setup(u => u.GetRolesAsync(user)).ReturnsAsync(new List<string> { "Customer" });
            _users.Setup(u => u.IsLockedOutAsync(user)).ReturnsAsync(false);
            _users.Setup(u => u.IsEmailConfirmedAsync(user)).ReturnsAsync(true);
            _users.Setup(u => u.CheckPasswordAsync(user, "wrong")).ReturnsAsync(false);
            _users.Setup(u => u.AccessFailedAsync(user)).Returns(Task.CompletedTask);

            var svc = CreateService();
            await Assert.ThrowsAsync<BadRequestException>(() =>
                svc.Login(new LoginRequestDto { UserName = "x@y.com", Password = "wrong" }));

            _users.Verify(u => u.AccessFailedAsync(user), Times.Once);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsToken()
        {
            var user = new ApplicationUser { Id = "u1", Email = "x@y.com", UserName = "x@y.com" };
            _users.Setup(u => u.GetByEmailAsync("x@y.com")).ReturnsAsync(user);
            _users.Setup(u => u.GetRolesAsync(user)).ReturnsAsync(new List<string> { "Customer" });
            _users.Setup(u => u.IsLockedOutAsync(user)).ReturnsAsync(false);
            _users.Setup(u => u.IsEmailConfirmedAsync(user)).ReturnsAsync(true);
            _users.Setup(u => u.CheckPasswordAsync(user, "pass")).ReturnsAsync(true);
            _users.Setup(u => u.ResetAccessFailedCountAsync(user)).Returns(Task.CompletedTask);
            _jwt.Setup(j => j.GenerateToken(user, It.IsAny<IEnumerable<string>>())).Returns("valid-token");

            var svc = CreateService();
            var result = await svc.Login(new LoginRequestDto { UserName = "x@y.com", Password = "pass" });

            Assert.Equal("valid-token", result.Token);
        }

        // ---- Register ----

        [Fact]
        public async Task Register_SuccessfulRegistration_SendsActivationEmail()
        {
            var user = new ApplicationUser { Id = "new-user-id", Email = "new@user.com", UserName = "new@user.com" };
            var dto = new RegistrationRequestDto { Email = "new@user.com", Password = "P@ss1" };

            _mapper.Setup(m => m.Map<ApplicationUser>(dto)).Returns(user);
            _users.Setup(u => u.CreateAsync(user, dto.Password)).ReturnsAsync(IdentityResult.Success);
            _users.Setup(u => u.EnsureRoleExistsAsync("Customer")).Returns(Task.CompletedTask);
            _users.Setup(u => u.AddToRoleAsync(user, "Customer")).Returns(Task.CompletedTask);
            _users.Setup(u => u.LockAsync(user)).Returns(Task.CompletedTask);

            // Build activation URI requires HttpContext.Request
            var request = new Mock<HttpRequest>();
            request.Setup(r => r.Scheme).Returns("https");
            request.Setup(r => r.Host).Returns(new HostString("example.com"));
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(c => c.Request).Returns(request.Object);
            _httpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext.Object);

            _users.Setup(u => u.GenerateEmailConfirmationTokenAsync(user)).ReturnsAsync("token");
            _notification
                .Setup(n => n.SendAsync(dto.Email, It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new SendEmailResponse(It.IsAny<bool>(), It.IsAny<string>()));

            var svc = CreateService();
            var resultId = await svc.Register(dto);

            Assert.Equal("new-user-id", resultId);
            _notification.Verify(n => n.SendAsync(dto.Email, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Register_CreateFails_ThrowsBadRequestException()
        {
            var user = new ApplicationUser { Id = "", Email = "new@user.com", UserName = "new@user.com" };
            var dto = new RegistrationRequestDto { Email = "new@user.com", Password = "P@ss1" };

            _mapper.Setup(m => m.Map<ApplicationUser>(dto)).Returns(user);
            _users.Setup(u => u.CreateAsync(user, dto.Password))
                  .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Weak password" }));

            var svc = CreateService();
            await Assert.ThrowsAsync<BadRequestException>(() => svc.Register(dto));
        }

        // ---- TryUnlockUserAccount ----

        [Fact]
        public async Task TryUnlockUserAccount_UserNotFound_ThrowsNotFoundException()
        {
            _users.Setup(u => u.GetByIdAsync("missing")).ReturnsAsync((ApplicationUser)null);

            var svc = CreateService();
            await Assert.ThrowsAsync<NotFoundException>(() => svc.TryUnlockUserAccount("missing"));
        }

        [Fact]
        public async Task TryUnlockUserAccount_ValidUser_UnlocksAndReturnsTrue()
        {
            var user = new ApplicationUser { Id = "u1" };
            _users.Setup(u => u.GetByIdAsync("u1")).ReturnsAsync(user);
            _users.Setup(u => u.UnlockAsync(user)).Returns(Task.CompletedTask);

            var svc = CreateService();
            var result = await svc.TryUnlockUserAccount("u1");

            Assert.True(result);
            _users.Verify(u => u.UnlockAsync(user), Times.Once);
        }

        // ---- TryLockUserAccount ----

        [Fact]
        public async Task TryLockUserAccount_UserNotFound_ThrowsNotFoundException()
        {
            _users.Setup(u => u.GetByIdAsync("missing")).ReturnsAsync((ApplicationUser)null);

            var svc = CreateService();
            await Assert.ThrowsAsync<NotFoundException>(() => svc.TryLockUserAccount("missing"));
        }

        [Fact]
        public async Task TryLockUserAccount_ValidUser_LocksAndReturnsTrue()
        {
            var user = new ApplicationUser { Id = "u1" };
            _users.Setup(u => u.GetByIdAsync("u1")).ReturnsAsync(user);
            _users.Setup(u => u.LockAsync(user)).Returns(Task.CompletedTask);

            var svc = CreateService();
            var result = await svc.TryLockUserAccount("u1");

            Assert.True(result);
            _users.Verify(u => u.LockAsync(user), Times.Once);
        }

        // ---- TryActivateUserAsync ----

        [Fact]
        public async Task TryActivateUserAsync_UserNotFound_ThrowsNotFoundException()
        {
            _users.Setup(u => u.GetByIdAsync("missing")).ReturnsAsync((ApplicationUser)null);

            var svc = CreateService();
            await Assert.ThrowsAsync<NotFoundException>(() => svc.TryActivateUserAsync("missing", "token"));
        }
    }

    // =========================================================================
    // CloudinaryImageService Tests
    // =========================================================================

    public class CloudinaryImageServiceTests
    {
        // NOTE: Cloudinary is a sealed class so we test the service's guard-clause
        // logic (without hitting the real API) and verify method behavior.

        private static IFormFile BuildFormFile(string name = "test.jpg", long size = 100)
        {
            var mock = new Mock<IFormFile>();
            mock.Setup(f => f.FileName).Returns(name);
            mock.Setup(f => f.Length).Returns(size);
            mock.Setup(f => f.OpenReadStream()).Returns(new System.IO.MemoryStream(new byte[size]));
            return mock.Object;
        }

        [Fact]
        public async Task DeleteAsync_EmptyPublicId_ReturnsFalse()
        {
            var cloudinary = new Cloudinary("cloudinary://key:secret@cloud");
            var svc = new CloudinaryImageService(cloudinary);

            var result = await svc.DeleteAsync(string.Empty);

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_NullPublicId_ReturnsFalse()
        {
            var cloudinary = new Cloudinary("cloudinary://key:secret@cloud");
            var svc = new CloudinaryImageService(cloudinary);

            var result = await svc.DeleteAsync(null);

            Assert.False(result);
        }

        [Fact]
        public async Task UpdateAsync_EmptyPublicId_ThrowsBadRequestException()
        {
            var cloudinary = new Cloudinary("cloudinary://key:secret@cloud");
            var svc = new CloudinaryImageService(cloudinary);

            await Assert.ThrowsAsync<BadRequestException>(() =>
                svc.UpdateAsync(string.Empty, 100, 100, BuildFormFile()));
        }

        [Fact]
        public async Task UploadAsync_NullFile_ThrowsBadRequestException()
        {
            var cloudinary = new Cloudinary("cloudinary://key:secret@cloud");
            var svc = new CloudinaryImageService(cloudinary);

            await Assert.ThrowsAsync<BadRequestException>(() =>
                svc.UploadAsync(null, 200, 200, "test-folder"));
        }

        [Fact]
        public async Task UploadAsync_EmptyFile_ThrowsBadRequestException()
        {
            var cloudinary = new Cloudinary("cloudinary://key:secret@cloud");
            var svc = new CloudinaryImageService(cloudinary);

            var emptyFile = BuildFormFile(size: 0);
            await Assert.ThrowsAsync<BadRequestException>(() =>
                svc.UploadAsync(emptyFile, 200, 200, "test-folder"));
        }
    }
}
