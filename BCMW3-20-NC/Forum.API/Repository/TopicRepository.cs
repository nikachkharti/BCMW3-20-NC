using Forum.API.Data;
using Forum.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Forum.API.Repository
{
    public class TopicRepository : ITopicRepository
    {
        private readonly ApplicationDbContext _context;

        public TopicRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task AddNewTopicAsync(Topic entity)
        {
            throw new NotImplementedException();
        }

        public Task<Topic> DeleteSingleTopicAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Topic>> GetAllTopicsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Topic> GetSingleTopicAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateNewTopicAsync(Topic entity)
        {
            throw new NotImplementedException();
        }
    }
}
