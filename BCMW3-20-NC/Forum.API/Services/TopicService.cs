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
            await _topicRepository.AddAsync(_mapper.Map<Topic>(model));
            return await _topicRepository.SaveAsync();
        }

        public async Task<int> DeleteTopicAsync(Guid topicId)
        {
            var topicToDelete = await _topicRepository.GetAsync(t => t.Id == topicId);

            if (topicToDelete != null)
            {
                _topicRepository.Remove(topicToDelete);
                return await _topicRepository.SaveAsync();
            }

            return 0;
        }

        public async Task<(List<TopicListForGettingDto>, int totalCount)> GetAllTopicsAsync(int? pageNumber, int? pageSize)
        {
            var result = await _topicRepository.GetAllAsync(pageNumber: pageNumber, pageSize: pageSize);

            if (result.Items.Count > 0)
            {
                var mappedResult = _mapper.Map<List<TopicListForGettingDto>>(result.Items);
                return (mappedResult, result.TotalCount);
            }

            return (Enumerable
                .Empty<TopicListForGettingDto>()
                .ToList(), 0);
        }

        public async Task<TopicDetailsForGettingDto> GetOpicDetailsAsync(Guid topicId)
        {
            var result = await _topicRepository.GetAsync(t => t.Id == topicId, includeProperties: "Comments");
            return _mapper.Map<TopicDetailsForGettingDto>(result);
        }

        public async Task<int> UpdateNewTopicAsync(TopicForUpdatingDto model)
        {
            var topicToUpdate = await _topicRepository.GetAsync(t => t.Id == model.Id);

            if (topicToUpdate != null)
            {
                _topicRepository.Update(topicToUpdate);
                return await _topicRepository.SaveAsync();
            }

            return 0;
        }

    }
}
