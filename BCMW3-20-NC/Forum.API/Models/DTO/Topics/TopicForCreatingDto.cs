namespace Forum.API.Models.DTO.Topics
{
    public record TopicForCreatingDto
    (
        string Title,
        string Content,
        string ImageUrl
        //DateTime CreateDate,
        //DateTime LastCommentDate,
        //bool CommentsAreAllowed
    );
}
