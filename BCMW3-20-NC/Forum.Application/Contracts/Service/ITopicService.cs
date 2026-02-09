using Forum.API.Application.DTO.Topics;

namespace Forum.Application.Contracts.Service
{
    public interface ITopicService
    {
        Task<(List<TopicListForGettingDto> Topics, int TotalCount)> GetAllTopicsAsync(
            int? pageNumber,
            int? pageSize
        );
        Task<TopicDetailsForGettingDto> GetTopicDetailsAsync(Guid topicId);
        Task<int> AddNewTopicAsync(TopicForCreatingDto model);
        Task<int> UpdateTopicAsync(TopicForUpdatingDto model);
        Task<int> DeleteTopicAsync(Guid topicId);
    }
}
