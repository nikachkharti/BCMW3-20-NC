using System.ComponentModel.DataAnnotations;

namespace Forum.API.Models.DTO.Comments
{
    public record CommentForUpdatingDto
    (
        [Required]
        Guid Id,

        [Required]
        string Content
    );
}
