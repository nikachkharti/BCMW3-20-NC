using Forum.API.Entities;

namespace Forum.API.Repository
{
    public interface ITopicRepository
    {
        Task<List<Topic>> GetAllTopicsAsync();
    }
}
