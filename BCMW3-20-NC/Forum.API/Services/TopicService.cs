using Forum.API.Models.DTO.Topics;
using Forum.API.Repository;

namespace Forum.API.Services
{
    public class TopicService : ITopicService
    {
        private readonly ITopicRepository _topicRepository;

        public TopicService(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }

        public Task<int> AddNewTopicAsync(TopicForCreatingDto model)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteTopicAsync(Guid topicId)
        {
            throw new NotImplementedException();
        }

        public Task<List<TopicListForGettingDto>> GetAllTopicsAsync(int? pageNumber = 1, int? pageSize = 10)
        {
            throw new NotImplementedException();
        }

        public Task<TopicDetailsForGettingDto> GetOpicDetailsAsync(Guid topicId)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateNewTopicAsync(TopicForUpdatingDto model)
        {
            throw new NotImplementedException();
        }
    }
}
