namespace Forum.API.Models.DTO.Comments
{
    public record CommentForGettingDto
    (
         string Id,
         string Content,
         DateTime CommentDate
    );
}
