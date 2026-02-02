using System.ComponentModel.DataAnnotations;

namespace Forum.API.Models.DTO.Comments
{
    public record CommentForCreatingDto
    (
        [Required]
         string Content,

        [Required]
         string TopicId
        //DateTime CommentDate
    );
}
