using Forum.API.Application.DTO.Topics;
using Forum.Application.Contracts.Repository;
using Forum.Application.Contracts.Service;
using Forum.Application.Exceptions;
using Forum.Domain.Entities;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Forum.Application.Services
{
    public class TopicService : ITopicService
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICloudinaryImageService _cloudinaryImageService;
        private const int _width = 200;
        private const int _height = 200;


        public TopicService(ITopicRepository topicRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, ICloudinaryImageService cloudinaryImageService)
        {
            _topicRepository = topicRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _cloudinaryImageService = cloudinaryImageService;
        }

        public async Task<int> AddNewTopicAsync(TopicForCreatingDto model)
        {
            ValidateCreateModel(model);
            var uploadResult = await _cloudinaryImageService.UploadAsync(model.Avatar, _width, _height, folder: "topics");

            var authenticatedUserId = AuthenticatedUserId();

            if (string.IsNullOrWhiteSpace(authenticatedUserId))
                throw new ForbidException("Unable to add topic for unauthorzied user");

            var entity = _mapper.Map<Topic>(model);
            entity.AuthorId = authenticatedUserId;
            entity.ImageUrl = uploadResult.Url;
            entity.ImagePublicId = uploadResult.PublicId;

            try
            {
                await _topicRepository.AddAsync(entity);
                return await _topicRepository.SaveAsync();
            }
            catch
            {
                await _cloudinaryImageService.DeleteAsync(uploadResult.PublicId);
                throw;
            }
        }

        public async Task<int> DeleteTopicAsync(Guid topicId)
        {
            ValidateGuid(topicId);

            var topic = await _topicRepository.GetAsync(t => t.Id == topicId);

            if (topic == null)
                throw new ArgumentException($"Topic with id '{topicId}' not found.");

            if (!UserCanModifyContent(topic))
                throw new ForbidException($"Authenticated user have no permission");

            _topicRepository.Remove(topic);

            int result = await _topicRepository.SaveAsync();

            if (result > 0 && !string.IsNullOrWhiteSpace(topic.ImagePublicId))
                await _cloudinaryImageService.DeleteAsync(topic.ImagePublicId);

            return result;
        }

        public async Task<(List<TopicListForGettingDto> Topics, int TotalCount)> GetAllTopicsAsync(
            int? pageNumber,
            int? pageSize)
        {
            ValidatePaging(pageNumber, pageSize);

            var result = await _topicRepository.GetAllAsync(
                pageNumber: pageNumber,
                pageSize: pageSize,
                orderBy: "CreateDate",
                ascending: false
            );

            var topics = _mapper.Map<List<TopicListForGettingDto>>(result.Items);
            return (topics, result.TotalCount);
        }

        public async Task<TopicDetailsForGettingDto> GetTopicDetailsAsync(Guid topicId)
        {
            ValidateGuid(topicId);

            var topic = await _topicRepository.GetAsync(
                t => t.Id == topicId,
                includeProperties: "Comments"
            );

            if (topic == null)
                throw new ArgumentException($"Topic with id '{topicId}' not found.");

            return _mapper.Map<TopicDetailsForGettingDto>(topic);
        }

        public async Task<int> UpdateTopicAsync(TopicForUpdatingDto model)
        {
            ValidateUpdateModel(model);

            var topic = await _topicRepository.GetAsync(t => t.Id == model.Id);

            if (topic == null)
                throw new ArgumentException($"Topic with id '{model.Id}' not found.");

            if (!UserCanModifyContent(topic))
                throw new ForbidException($"Authenticated user have no permission");

            _mapper.Map(model, topic);
            return await _topicRepository.SaveAsync();
        }


        #region HELPERS

        private bool UserCanModifyContent(Topic content)
        {
            string authenticatedUserId = AuthenticatedUserId();

            if (string.IsNullOrWhiteSpace(authenticatedUserId))
                return false;

            if (content.AuthorId.Trim() != authenticatedUserId.Trim())
                return false;

            return true;
        }

        private string AuthenticatedUserId() =>
            _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true
                ? _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
                : string.Empty;


        #endregion


        #region VALIDATORS

        private static void ValidateGuid(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Invalid id.");
        }

        private static void ValidateCreateModel(TopicForCreatingDto model)
        {
            if (model == null)
                throw new ArgumentException("Request body is required.");

            if (string.IsNullOrWhiteSpace(model.Title))
                throw new ArgumentException("Title is required.");

            if (string.IsNullOrWhiteSpace(model.Content))
                throw new ArgumentException("Content is required.");

            if (model.Avatar is null || model.Avatar.Length == 0)
                throw new BadRequestException("Image is required parameter");
        }

        private static void ValidateUpdateModel(TopicForUpdatingDto model)
        {
            if (model == null)
                throw new ArgumentException("Request body is required.");

            ValidateGuid(model.Id);

            if (string.IsNullOrWhiteSpace(model.Title))
                throw new ArgumentException("Title is required.");

            if (string.IsNullOrWhiteSpace(model.Content))
                throw new ArgumentException("Content is required.");
        }

        private static void ValidatePaging(int? pageNumber, int? pageSize)
        {
            if (pageNumber <= 0)
                throw new ArgumentException("PageNumber must be greater than 0.");

            if (pageSize <= 0)
                throw new ArgumentException("PageSize must be greater than 0.");
        }

        #endregion
    }
}
