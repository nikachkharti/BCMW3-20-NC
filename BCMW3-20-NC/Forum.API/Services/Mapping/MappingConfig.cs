using Forum.API.Entities;
using Forum.API.Models.DTO.Comments;
using Forum.API.Models.DTO.Topics;
using Mapster;

namespace Forum.API.Services.Mapping
{
    public static class MappingConfig
    {
        public static void RegisterMappings(TypeAdapterConfig config)
        {
            config.NewConfig<TopicForCreatingDto, Topic>()
                .Map(dest => dest.Title, src => src.Title)
                .Map(dest => dest.Content, src => src.Content)
                .Map(dest => dest.ImageUrl, src => src.ImageUrl);

            config.NewConfig<Topic, TopicListForGettingDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Title, src => src.Title)
                .Map(dest => dest.CreateDate, src => src.CreateDate);

            config.NewConfig<Topic, TopicDetailsForGettingDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Title, src => src.Title)
                .Map(dest => dest.Content, src => src.Content)
                .Map(dest => dest.CreateDate, src => src.CreateDate)
                .Map(dest => dest.ImageUrl, src => src.ImageUrl)
                .Map(dest => dest.LastCommentDate, src => src.LastCommentDate)
                .Map(dest => dest.CommentsAreAllowed, src => src.CommentsAreAllowed)
                .Map(dest => dest.Comments, src => src.Comments);


            config.NewConfig<Comment, CommentForGettingDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Content, src => src.Content)
                .Map(dest => dest.CommentDate, src => src.CommentDate);
        }
    }
}
