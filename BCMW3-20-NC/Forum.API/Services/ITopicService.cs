using Forum.API.Models.DTO.Topics;

namespace Forum.API.Services
{
    public interface ITopicService
    {
        Task<(List<TopicListForGettingDto> Topics, int TotalCount)> GetAllTopicsAsync(
            int? pageNumber,
            int? pageSize
        );
        Task<TopicDetailsForGettingDto> GetTopicDetailsAsync(Guid topicId);
        Task<int> AddNewTopicAsync(TopicForCreatingDto model);
        Task<int> UpdateNewTopicAsync(TopicForUpdatingDto model);
        Task<int> DeleteTopicAsync(Guid topicId);
    }
}
