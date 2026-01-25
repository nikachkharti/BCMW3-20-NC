using Forum.API.Entities;

namespace Forum.API.Repository
{
    public interface ITopicRepository
    {
        Task<List<Topic>> GetAllTopicsAsync();
        Task<Topic> GetSingleTopicAsync(Guid id);
        Task AddNewTopicAsync(Topic entity);
        Task UpdateNewTopicAsync(Topic entity);
        Task<Topic> DeleteSingleTopicAsync(Guid id);
    }
}
