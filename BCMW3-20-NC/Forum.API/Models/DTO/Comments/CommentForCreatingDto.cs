namespace Forum.API.Models.DTO.Comments
{
    public record CommentForCreatingDto
    (
         string Content,
         Guid TopicId
        //DateTime CommentDate
    );
}
