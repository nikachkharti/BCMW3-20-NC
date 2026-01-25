using Forum.API.Entities;

namespace Forum.API.Repository
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllCommentsAsync();
        Task<Comment> GetSingleCommentAsync(Guid id);
        Task AddNewCommentAsync(Comment entity);
        Task UpdateNewCommentAsync(Comment entity);
        Task<Comment> DeleteSingleCommentAsync(Guid id);
    }
}
