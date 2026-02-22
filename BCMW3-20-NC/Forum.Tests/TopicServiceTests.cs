using Forum.API.Application.DTO.Topics;
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
using System.Linq.Expressions;

namespace Forum.Tests
{
    // =========================================================================
    // TopicService Tests
    // =========================================================================

    public class TopicServiceTests
    {
        private readonly Mock<ITopicRepository> _topicRepo = new();
        private readonly Mock<IMapper> _mapper = new();
        private readonly Mock<ICloudinaryImageService> _cloudinary = new();

        private TopicService CreateService(string userId = "user-1") =>
            new(_topicRepo.Object, _mapper.Object, HttpContextFactory.Authenticated(userId), _cloudinary.Object);

        private TopicService CreateUnauthenticatedService() =>
            new(_topicRepo.Object, _mapper.Object, HttpContextFactory.Unauthenticated(), _cloudinary.Object);

        // ---- AddNewTopicAsync ----

        [Fact]
        public async Task AddNewTopicAsync_NullModel_ThrowsArgumentException()
        {
            var svc = CreateService();
            await Assert.ThrowsAsync<ArgumentException>(() => svc.AddNewTopicAsync(null));
        }

        [Fact]
        public async Task AddNewTopicAsync_MissingTitle_ThrowsArgumentException()
        {
            var svc = CreateService();
            var model = new TopicForCreatingDto(Title: "", Content: "c", Avatar: Mock.Of<IFormFile>(f => f.Length == 1));
            await Assert.ThrowsAsync<ArgumentException>(() => svc.AddNewTopicAsync(model));
        }

        [Fact]
        public async Task AddNewTopicAsync_MissingContent_ThrowsArgumentException()
        {
            var svc = CreateService();
            var model = new TopicForCreatingDto(Title: "t", Content: "", Avatar: Mock.Of<IFormFile>(f => f.Length == 1));
            await Assert.ThrowsAsync<ArgumentException>(() => svc.AddNewTopicAsync(model));
        }

        [Fact]
        public async Task AddNewTopicAsync_MissingAvatar_ThrowsBadRequestException()
        {
            var svc = CreateService();
            var model = new TopicForCreatingDto(Title: "t", Content: "c", Avatar: null);
            await Assert.ThrowsAsync<BadRequestException>(() => svc.AddNewTopicAsync(model));
        }

        [Fact]
        public async Task AddNewTopicAsync_Unauthenticated_ThrowsForbidException()
        {
            var svc = CreateUnauthenticatedService();
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(1);

            _cloudinary.Setup(c => c.UploadAsync(It.IsAny<IFormFile>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), default))
                       .ReturnsAsync(new ImageUploadResultDto { Url = "http://img", PublicId = "pid" });

            var model = new TopicForCreatingDto(Title: "t", Content: "c", Avatar: fileMock.Object);
            await Assert.ThrowsAsync<ForbidException>(() => svc.AddNewTopicAsync(model));
        }

        [Fact]
        public async Task AddNewTopicAsync_ValidModel_ReturnsSaveResult()
        {
            const string userId = "user-1";
            var svc = CreateService(userId);
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(1);

            var uploadResult = new ImageUploadResultDto { Url = "http://img", PublicId = "pid" };
            _cloudinary.Setup(c => c.UploadAsync(It.IsAny<IFormFile>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), default))
                       .ReturnsAsync(uploadResult);

            var entity = new Topic();
            _mapper.Setup(m => m.Map<Topic>(It.IsAny<TopicForCreatingDto>())).Returns(entity);
            _topicRepo.Setup(r => r.AddAsync(entity)).Returns(Task.CompletedTask);
            _topicRepo.Setup(r => r.SaveAsync()).ReturnsAsync(1);

            var model = new TopicForCreatingDto(Title: "t", Content: "c", Avatar: fileMock.Object);
            var result = await svc.AddNewTopicAsync(model);

            Assert.Equal(1, result);
            Assert.Equal(userId, entity.AuthorId);
            Assert.Equal("http://img", entity.ImageUrl);
        }

        [Fact]
        public async Task AddNewTopicAsync_SaveThrows_DeletesUploadedImage()
        {
            const string userId = "user-1";
            var svc = CreateService(userId);
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(1);

            var uploadResult = new ImageUploadResultDto { Url = "http://img", PublicId = "pid" };
            _cloudinary.Setup(c => c.UploadAsync(It.IsAny<IFormFile>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), default))
                       .ReturnsAsync(uploadResult);

            _mapper.Setup(m => m.Map<Topic>(It.IsAny<TopicForCreatingDto>())).Returns(new Topic());
            _topicRepo.Setup(r => r.AddAsync(It.IsAny<Topic>())).Returns(Task.CompletedTask);
            _topicRepo.Setup(r => r.SaveAsync()).ThrowsAsync(new Exception("DB error"));

            var model = new TopicForCreatingDto(Title: "t", Content: "c", Avatar: fileMock.Object);
            await Assert.ThrowsAsync<Exception>(() => svc.AddNewTopicAsync(model));

            _cloudinary.Verify(c => c.DeleteAsync("pid", It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        // ---- DeleteTopicAsync ----

        [Fact]
        public async Task DeleteTopicAsync_EmptyGuid_ThrowsArgumentException()
        {
            var svc = CreateService();
            await Assert.ThrowsAsync<ArgumentException>(() => svc.DeleteTopicAsync(Guid.Empty));
        }

        [Fact]
        public async Task DeleteTopicAsync_TopicNotFound_ThrowsArgumentException()
        {
            var svc = CreateService();
            _topicRepo.Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Topic, bool>>>(), It.IsAny<Func<IQueryable<Topic>, IQueryable<Topic>>>()))
                      .ReturnsAsync((Topic)null);

            await Assert.ThrowsAsync<ArgumentException>(() => svc.DeleteTopicAsync(Guid.NewGuid()));
        }

        [Fact]
        public async Task DeleteTopicAsync_DifferentAuthor_ThrowsForbidException()
        {
            var svc = CreateService("user-1");
            var topic = new Topic { Id = Guid.NewGuid(), AuthorId = "user-2" };
            _topicRepo.Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Topic, bool>>>(), It.IsAny<Func<IQueryable<Topic>, IQueryable<Topic>>>()))
                      .ReturnsAsync(topic);

            await Assert.ThrowsAsync<ForbidException>(() => svc.DeleteTopicAsync(topic.Id));
        }

        [Fact]
        public async Task DeleteTopicAsync_ValidOwner_DeletesAndReturns()
        {
            const string userId = "user-1";
            var svc = CreateService(userId);
            var topicId = Guid.NewGuid();
            var topic = new Topic { Id = topicId, AuthorId = userId, ImagePublicId = "pub-id" };

            _topicRepo.Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Topic, bool>>>(), It.IsAny<Func<IQueryable<Topic>, IQueryable<Topic>>>()))
                      .ReturnsAsync(topic);
            _topicRepo.Setup(r => r.Remove(topic));
            _topicRepo.Setup(r => r.SaveAsync()).ReturnsAsync(1);
            _cloudinary.Setup(c => c.DeleteAsync("pub-id", It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var result = await svc.DeleteTopicAsync(topicId);

            Assert.Equal(1, result);
            _cloudinary.Verify(c => c.DeleteAsync("pub-id", It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        // ---- UpdateTopicAsync ----

        [Fact]
        public async Task UpdateTopicAsync_NullModel_ThrowsArgumentException()
        {
            var svc = CreateService();
            await Assert.ThrowsAsync<ArgumentException>(() => svc.UpdateTopicAsync(null));
        }

        [Fact]
        public async Task UpdateTopicAsync_TopicNotFound_ThrowsArgumentException()
        {
            var svc = CreateService();
            _topicRepo.Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Topic, bool>>>(), It.IsAny<Func<IQueryable<Topic>, IQueryable<Topic>>>()))
                      .ReturnsAsync((Topic)null);

            var model = new TopicForUpdatingDto(Id: Guid.NewGuid(), Title: "t", Content: "c", ImageUrl: null, CommentsAreAllowed: false);
            await Assert.ThrowsAsync<ArgumentException>(() => svc.UpdateTopicAsync(model));
        }

        [Fact]
        public async Task UpdateTopicAsync_DifferentAuthor_ThrowsForbidException()
        {
            var svc = CreateService("user-1");
            var topic = new Topic { Id = Guid.NewGuid(), AuthorId = "user-2" };
            _topicRepo.Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Topic, bool>>>(), It.IsAny<Func<IQueryable<Topic>, IQueryable<Topic>>>()))
                      .ReturnsAsync(topic);

            var model = new TopicForUpdatingDto(Id: topic.Id, Title: "t", Content: "c", ImageUrl: null, CommentsAreAllowed: false);
            await Assert.ThrowsAsync<ForbidException>(() => svc.UpdateTopicAsync(model));
        }

        [Fact]
        public async Task UpdateTopicAsync_ValidOwner_ReturnsSaveResult()
        {
            const string userId = "user-1";
            var svc = CreateService(userId);
            var topic = new Topic { Id = Guid.NewGuid(), AuthorId = userId };
            _topicRepo.Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Topic, bool>>>(), It.IsAny<Func<IQueryable<Topic>, IQueryable<Topic>>>()))
                      .ReturnsAsync(topic);
            _topicRepo.Setup(r => r.SaveAsync()).ReturnsAsync(1);
            _mapper.Setup(m => m.Map(It.IsAny<TopicForUpdatingDto>(), topic));

            var model = new TopicForUpdatingDto(Id: topic.Id, Title: "Updated", Content: "Updated content", ImageUrl: null, CommentsAreAllowed: true);
            var result = await svc.UpdateTopicAsync(model);

            Assert.Equal(1, result);
        }

        // ---- GetAllTopicsAsync ----

        [Fact]
        public async Task GetAllTopicsAsync_InvalidPageNumber_ThrowsArgumentException()
        {
            var svc = CreateService();
            await Assert.ThrowsAsync<ArgumentException>(() => svc.GetAllTopicsAsync(0, 10));
        }

        [Fact]
        public async Task GetAllTopicsAsync_InvalidPageSize_ThrowsArgumentException()
        {
            var svc = CreateService();
            await Assert.ThrowsAsync<ArgumentException>(() => svc.GetAllTopicsAsync(1, 0));
        }

    }

}
