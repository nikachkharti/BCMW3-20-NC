using Forum.API.Models.DTO.Topics;

namespace Forum.API.Services
{
    public interface ITopicService
    {
        Task<List<TopicListForGettingDto>> GetAllTopicsAsync(int? pageNumber = 1, int? pageSize = 10);
        Task<TopicDetailsForGettingDto> GetOpicDetailsAsync(Guid topicId);
        Task<int> AddNewTopicAsync(TopicForCreatingDto model);
        Task<int> UpdateNewTopicAsync(TopicForUpdatingDto model);
        Task<int> DeleteTopicAsync(Guid topicId);
    }
}
