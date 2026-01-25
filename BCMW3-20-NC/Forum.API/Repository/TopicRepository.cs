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

        public async Task AddNewTopicAsync(Topic entity)
        {
            await _context.Topics.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<Topic> DeleteSingleTopicAsync(Guid id)
        {
            var topic = await _context.Topics.FirstOrDefaultAsync(x => x.Id == id);

            if (topic == null)
                return null;

            _context.Topics.Remove(topic);
            await _context.SaveChangesAsync();

            return topic;
        }

        public async Task<List<Topic>> GetAllTopicsAsync()
        {
            return await _context.Topics.ToListAsync();
        }

        public async Task<Topic> GetSingleTopicAsync(Guid id)
        {
            return await _context.Topics.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateNewTopicAsync(Topic entity)
        {
            _context.Topics.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
