using Forum.API.Entities;
using Forum.API.Models.DTO.Topics;
using Forum.API.Repository;
using MapsterMapper;

namespace Forum.API.Services
{
    public class TopicService : ITopicService
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IMapper _mapper;

        public TopicService(ITopicRepository topicRepository, IMapper mapper)
        {
            _topicRepository = topicRepository;
            _mapper = mapper;
        }

        public async Task<int> AddNewTopicAsync(TopicForCreatingDto model)
        {
            ValidateCreateModel(model);

            var entity = _mapper.Map<Topic>(model);
            await _topicRepository.AddAsync(entity);

            return await _topicRepository.SaveAsync();
        }

        public async Task<int> DeleteTopicAsync(Guid topicId)
        {
            ValidateGuid(topicId);

            var topic = await _topicRepository.GetAsync(t => t.Id == topicId);
            if (topic == null)
                throw new ArgumentException($"Topic with id '{topicId}' not found.");

            _topicRepository.Remove(topic);
            return await _topicRepository.SaveAsync();
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

        public async Task<int> UpdateNewTopicAsync(TopicForUpdatingDto model)
        {
            ValidateUpdateModel(model);

            var topic = await _topicRepository.GetAsync(t => t.Id == model.Id);
            if (topic == null)
                throw new ArgumentException($"Topic with id '{model.Id}' not found.");

            _mapper.Map(model, topic);
            return await _topicRepository.SaveAsync();
        }



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
