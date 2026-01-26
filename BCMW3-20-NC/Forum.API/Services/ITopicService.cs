using Forum.API.Models.DTO.Topics;

namespace Forum.API.Services
{
    public interface ITopicService
    {
        Task<(List<TopicListForGettingDto>, int totalCount)> GetAllTopicsAsync(int? pageNumber, int? pageSize);
        Task<TopicDetailsForGettingDto> GetOpicDetailsAsync(Guid topicId);
        Task<int> AddNewTopicAsync(TopicForCreatingDto model);
        Task<int> UpdateNewTopicAsync(TopicForUpdatingDto model);
        Task<int> DeleteTopicAsync(Guid topicId);
    }
}
