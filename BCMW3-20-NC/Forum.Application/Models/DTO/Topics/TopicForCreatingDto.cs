using System.ComponentModel.DataAnnotations;

namespace Forum.API.Application.DTO.Topics
{
    public record TopicForCreatingDto
    (
        //[Required]
        //[MaxLength(50)]
        string Title,

        //[Required]
        string Content,

        string ImageUrl
        //DateTime CreateDate,
        //DateTime LastCommentDate,
        //bool CommentsAreAllowed
    );
}
