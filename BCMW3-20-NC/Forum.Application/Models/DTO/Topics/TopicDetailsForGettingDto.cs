using Forum.API.Models.DTO.Comments;

namespace Forum.API.Application.DTO.Topics
{
    public record TopicDetailsForGettingDto
    (
        Guid Id,
        string Title,
        string Content,
        DateTime CreateDate,
        string ImageUrl,
        string ImagePublicId,
        DateTime? LastCommentDate,
        bool CommentsAreAllowed,
        List<CommentForGettingDto> Comments
    );
}
