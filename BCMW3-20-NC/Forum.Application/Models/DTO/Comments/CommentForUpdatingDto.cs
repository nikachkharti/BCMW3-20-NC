using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Forum.API.Application.DTO.Comments
{
    public record CommentForUpdatingDto
    (
        [Required]
        Guid Id,

        [Required]
        string Content,

        IFormFile Image
    );
}
