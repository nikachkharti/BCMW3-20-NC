namespace Forum.API.Models.DTO.Topics
{
    public record TopicForUpdatingDto
    (
        Guid Id,
        string Title,
        string Content,
        string ImageUrl,
        bool CommentsAreAllowed
    );
}
