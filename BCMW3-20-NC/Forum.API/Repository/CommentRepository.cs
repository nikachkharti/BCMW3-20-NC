using Forum.API.Data;
using Forum.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Forum.API.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddNewCommentAsync(Comment entity)
        {
            await _context.Comments.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<Comment> DeleteSingleCommentAsync(Guid id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);

            if (comment == null)
                return null;

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return comment;
        }

        public async Task<List<Comment>> GetAllCommentsAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<Comment> GetSingleCommentAsync(Guid id)
        {
            return await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateNewCommentAsync(Comment entity)
        {
            _context.Comments.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
