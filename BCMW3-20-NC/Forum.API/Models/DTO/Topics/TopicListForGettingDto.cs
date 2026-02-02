namespace Forum.API.Models.DTO.Topics
{
    public record TopicListForGettingDto
    (
        string Id,
        string Title,
        DateTime CreateDate
    );
}
