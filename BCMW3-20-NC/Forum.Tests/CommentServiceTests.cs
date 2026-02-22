using Forum.API.Application.DTO.Comments;
using Forum.Application.Contracts.Repository;
using Forum.Application.Contracts.Service;
using Forum.Application.Exceptions;
using Forum.Application.Models.Cloudinary;
using Forum.Application.Services;
using Forum.Domain.Entities;
using Forum.Tests.Shared;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Forum.Tests
{
    // =========================================================================
    // CommentService Tests
    // =========================================================================

    public class CommentServiceTests
    {
        private readonly Mock<ICommentRepository> _commentRepo = new();
        private readonly Mock<IMapper> _mapper = new();
        private readonly Mock<ICloudinaryImageService> _cloudinary = new();

        private CommentService CreateService(string userId = "user-1") =>
            new(_commentRepo.Object, _mapper.Object, HttpContextFactory.Authenticated(userId), _cloudinary.Object);

        private CommentService CreateUnauthenticatedService() =>
            new(_commentRepo.Object, _mapper.Object, HttpContextFactory.Unauthenticated(), _cloudinary.Object);

        // ---- AddNewCommentAsync ----

        [Fact]
        public async Task AddNewCommentAsync_NullModel_ThrowsBadRequestException()
        {
            var svc = CreateService();
            await Assert.ThrowsAsync<BadRequestException>(() => svc.AddNewCommentAsync(null));
        }

        [Fact]
        public async Task AddNewCommentAsync_EmptyContent_ThrowsBadRequestException()
        {
            var svc = CreateService();
            var model = new CommentForCreatingDto(Content: "", TopicId: Guid.NewGuid(), null);
            await Assert.ThrowsAsync<BadRequestException>(() => svc.AddNewCommentAsync(model));
        }

        [Fact]
        public async Task AddNewCommentAsync_EmptyTopicId_ThrowsBadRequestException()
        {
            var svc = CreateService();
            var model = new CommentForCreatingDto(Content: "c", TopicId: Guid.Empty, null);
            await Assert.ThrowsAsync<BadRequestException>(() => svc.AddNewCommentAsync(model));
        }

        [Fact]
        public async Task AddNewCommentAsync_Unauthenticated_ThrowsForbidException()
        {
            var svc = CreateUnauthenticatedService();
            var model = new CommentForCreatingDto(Content: "c", TopicId: Guid.NewGuid(), null);
            await Assert.ThrowsAsync<ForbidException>(() => svc.AddNewCommentAsync(model));
        }

        [Fact]
        public async Task AddNewCommentAsync_ValidModel_ReturnsSaveResult()
        {
            const string userId = "user-1";
            var svc = CreateService(userId);
            var fileMock = new Mock<IFormFile>();
            var model = new CommentForCreatingDto(Content: "c", TopicId: Guid.NewGuid(), Image: fileMock.Object);

            var uploadResult = new ImageUploadResultDto { Url = "http://img", PublicId = "pid" };
            _cloudinary.Setup(c => c.UploadAsync(It.IsAny<IFormFile>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), default))
                       .ReturnsAsync(uploadResult);

            var entity = new Comment();
            _mapper.Setup(m => m.Map<Comment>(model)).Returns(entity);
            _commentRepo.Setup(r => r.AddAsync(entity)).Returns(Task.CompletedTask);
            _commentRepo.Setup(r => r.SaveAsync()).ReturnsAsync(1);

            var result = await svc.AddNewCommentAsync(model);

            Assert.Equal(1, result);
            Assert.Equal(userId, entity.AuthorId);
        }

        [Fact]
        public async Task AddNewCommentAsync_SaveThrows_DeletesUploadedImage()
        {
            const string userId = "user-1";
            var svc = CreateService(userId);
            var model = new CommentForCreatingDto(Content: "c", TopicId: Guid.NewGuid(), Image: Mock.Of<IFormFile>());

            _cloudinary.Setup(c => c.UploadAsync(It.IsAny<IFormFile>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), default))
                       .ReturnsAsync(new ImageUploadResultDto { Url = "http://img", PublicId = "pid" });
            _mapper.Setup(m => m.Map<Comment>(model)).Returns(new Comment());
            _commentRepo.Setup(r => r.AddAsync(It.IsAny<Comment>())).Returns(Task.CompletedTask);
            _commentRepo.Setup(r => r.SaveAsync()).ThrowsAsync(new Exception("DB error"));

            await Assert.ThrowsAsync<Exception>(() => svc.AddNewCommentAsync(model));
            _cloudinary.Verify(c => c.DeleteAsync("pid", It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        // ---- DeleteCommentAsync ----

        [Fact]
        public async Task DeleteCommentAsync_EmptyGuid_ThrowsArgumentException()
        {
            var svc = CreateService();
            await Assert.ThrowsAsync<ArgumentException>(() => svc.DeleteCommentAsync(Guid.Empty));
        }

        [Fact]
        public async Task DeleteCommentAsync_NotFound_ThrowsNotFoundException()
        {
            var svc = CreateService();
            _commentRepo.Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Comment, bool>>>(), It.IsAny<Func<IQueryable<Comment>, IQueryable<Comment>>>()))
                        .ReturnsAsync((Comment)null);

            await Assert.ThrowsAsync<NotFoundException>(() => svc.DeleteCommentAsync(Guid.NewGuid()));
        }

        [Fact]
        public async Task DeleteCommentAsync_DifferentAuthor_ThrowsForbidException()
        {
            var svc = CreateService("user-1");
            var comment = new Comment { Id = Guid.NewGuid(), AuthorId = "user-2" };
            _commentRepo.Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Comment, bool>>>(), It.IsAny<Func<IQueryable<Comment>, IQueryable<Comment>>>()))
                        .ReturnsAsync(comment);

            await Assert.ThrowsAsync<ForbidException>(() => svc.DeleteCommentAsync(comment.Id));
        }

        [Fact]
        public async Task DeleteCommentAsync_ValidOwner_DeletesImageAndReturns()
        {
            const string userId = "user-1";
            var svc = CreateService(userId);
            var comment = new Comment { Id = Guid.NewGuid(), AuthorId = userId, ImagePublicId = "pub-id" };

            _commentRepo.Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Comment, bool>>>(), It.IsAny<Func<IQueryable<Comment>, IQueryable<Comment>>>()))
                        .ReturnsAsync(comment);
            _commentRepo.Setup(r => r.Remove(comment));
            _commentRepo.Setup(r => r.SaveAsync()).ReturnsAsync(1);
            _cloudinary.Setup(c => c.DeleteAsync("pub-id", It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var result = await svc.DeleteCommentAsync(comment.Id);

            Assert.Equal(1, result);
            _cloudinary.Verify(c => c.DeleteAsync("pub-id", It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        // ---- UpdateCommentAsync ----

        [Fact]
        public async Task UpdateCommentAsync_NullModel_ThrowsBadRequestException()
        {
            var svc = CreateService();
            await Assert.ThrowsAsync<BadRequestException>(() => svc.UpdateCommentAsync(null));
        }

        [Fact]
        public async Task UpdateCommentAsync_EmptyContent_ThrowsBadRequestException()
        {
            var svc = CreateService();
            var model = new CommentForUpdatingDto(Id: Guid.NewGuid(), Content: "", Image: null);
            await Assert.ThrowsAsync<BadRequestException>(() => svc.UpdateCommentAsync(model));
        }

        [Fact]
        public async Task UpdateCommentAsync_NotFound_ThrowsNotFoundException()
        {
            var svc = CreateService();
            _commentRepo.Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Comment, bool>>>(), It.IsAny<Func<IQueryable<Comment>, IQueryable<Comment>>>()))
                        .ReturnsAsync((Comment)null);

            var model = new CommentForUpdatingDto(Id: Guid.NewGuid(), Content: "updated", Image: null);
            await Assert.ThrowsAsync<NotFoundException>(() => svc.UpdateCommentAsync(model));
        }

        [Fact]
        public async Task UpdateCommentAsync_DifferentAuthor_ThrowsForbidException()
        {
            var svc = CreateService("user-1");
            var comment = new Comment { Id = Guid.NewGuid(), AuthorId = "user-2" };
            _commentRepo.Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Comment, bool>>>(), It.IsAny<Func<IQueryable<Comment>, IQueryable<Comment>>>()))
                        .ReturnsAsync(comment);

            var model = new CommentForUpdatingDto(Id: comment.Id, Content: "updated", Image: null);
            await Assert.ThrowsAsync<ForbidException>(() => svc.UpdateCommentAsync(model));
        }

        [Fact]
        public async Task UpdateCommentAsync_ContentOnly_UpdatesContentAndSaves()
        {
            const string userId = "user-1";
            var svc = CreateService(userId);
            var comment = new Comment { Id = Guid.NewGuid(), AuthorId = userId, Content = "old", ImagePublicId = "pid" };
            _commentRepo.Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Comment, bool>>>(), It.IsAny<Func<IQueryable<Comment>, IQueryable<Comment>>>()))
                        .ReturnsAsync(comment);
            _commentRepo.Setup(r => r.SaveAsync()).ReturnsAsync(1);

            var model = new CommentForUpdatingDto(Id: comment.Id, Content: "new content", Image: null);
            await svc.UpdateCommentAsync(model);

            Assert.Equal("new content", comment.Content);
            _cloudinary.Verify(c => c.UpdateAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IFormFile>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateCommentAsync_WithImage_UpdatesImage()
        {
            const string userId = "user-1";
            var svc = CreateService(userId);
            var comment = new Comment { Id = Guid.NewGuid(), AuthorId = userId, Content = "old", ImagePublicId = "old-pid" };
            _commentRepo.Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Comment, bool>>>(), It.IsAny<Func<IQueryable<Comment>, IQueryable<Comment>>>()))
                        .ReturnsAsync(comment);
            _commentRepo.Setup(r => r.SaveAsync()).ReturnsAsync(1);

            var newImg = Mock.Of<IFormFile>();
            _cloudinary.Setup(c => c.UpdateAsync("old-pid", It.IsAny<int>(), It.IsAny<int>(), newImg, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync(new ImageUploadResultDto { Url = "http://new-img", PublicId = "new-pid" });

            var model = new CommentForUpdatingDto(Id: comment.Id, Content: "new content", Image: newImg);
            await svc.UpdateCommentAsync(model);

            Assert.Equal("http://new-img", comment.ImageUrl);
            Assert.Equal("new-pid", comment.ImagePublicId);
        }
    }
}
