using System.ComponentModel.DataAnnotations;

namespace Forum.API.Models.DTO.Comments
{
    public record CommentForUpdatingDto
    (
        [Required]
        string Id,

        [Required]
        string Content
    );
}
