namespace Forum.API.Application.DTO.Topics
{
    public record TopicListForGettingDto
    (
        Guid Id,
        string Title,
        DateTime CreateDate
    );
}
