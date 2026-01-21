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

        public async Task<List<Topic>> GetAllTopicsAsync()
        {
            return await _context.Topics.ToListAsync();
        }
    }
}
