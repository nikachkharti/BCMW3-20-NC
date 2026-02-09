namespace Forum.API.Models.DTO.Comments
{
    public record CommentForGettingDto
    (
         Guid Id,
         string Content,
         DateTime CommentDate
    );
}
