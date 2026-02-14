using Forum.Application.Models.DTO.Comments;

namespace Forum.API.Models.DTO.Comments
{
    public record CommentForGettingDto
    (
         Guid Id,
         string Content,
         DateTime CommentDate,
         string ImageUrl,
         AuthorForGettingDto AuthorOfComment
    );
}
