using Forum.API.Models.DTO.Comments;

namespace Forum.API.Models.DTO.Topics
{
    public record TopicDetailsForGettingDto
    (
        Guid Id,
        string Title,
        string Content,
        DateTime CreateDate,
        string ImageUrl,
        DateTime LastCommentDate,
        bool CommentsAreAllowed,
        List<CommentForGettingDto> Comments
    );
}
