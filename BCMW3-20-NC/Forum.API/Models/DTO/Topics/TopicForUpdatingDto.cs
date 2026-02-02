using System.ComponentModel.DataAnnotations;

namespace Forum.API.Models.DTO.Topics
{
    public record TopicForUpdatingDto
    (
        [Required]
        string Id,

        [Required]
        [MaxLength(50)]
        string Title,

        [Required]
        string Content,

        string ImageUrl,

        [Required]
        bool CommentsAreAllowed
    );
}
