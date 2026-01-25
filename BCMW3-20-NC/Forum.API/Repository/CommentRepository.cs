using Forum.API.Entities;

namespace Forum.API.Repository
{
    public class CommentRepository : ICommentRepository
    {
        public Task AddNewCommentAsync(Comment entity)
        {
            throw new NotImplementedException();
        }

        public Task<Comment> DeleteSingleCommentAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Comment>> GetAllCommentsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Comment> GetSingleCommentAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateNewCommentAsync(Comment entity)
        {
            throw new NotImplementedException();
        }
    }
}
