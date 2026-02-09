using System.ComponentModel.DataAnnotations;

namespace Forum.API.Application.DTO.Comments
{
    public record CommentForCreatingDto
    (
        [Required]
         string Content,

        [Required]
         Guid TopicId
        //DateTime CommentDate
    );
}
