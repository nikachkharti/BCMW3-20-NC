using Forum.API.Application.DTO.Comments;

namespace Forum.Application.Contracts.Service
{
    public interface ICommentService
    {
        Task<int> AddNewCommentAsync(CommentForCreatingDto model);
        Task<int> UpdateCommentAsync(CommentForUpdatingDto model);
        Task<int> DeleteCommentAsync(Guid commentId);
    }
}