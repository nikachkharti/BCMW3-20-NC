using Forum.API.Application.DTO.Auth;
using Forum.API.Application.DTO.Comments;
using Forum.API.Application.DTO.Topics;
using Forum.API.Models.DTO.Comments;
using Forum.Application.Models.DTO.Comments;
using Forum.Domain.Entities;
using Mapster;

namespace Forum.Application.Mapping
{
    public static class MappingConfig
    {
        public static void RegisterMappings(TypeAdapterConfig config)
        {
            config.NewConfig<TopicForCreatingDto, Topic>()
                .Map(dest => dest.Title, src => src.Title)
                .Map(dest => dest.Content, src => src.Content);

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
                .Map(dest => dest.ImagePublicId, src => src.ImagePublicId)
                .Map(dest => dest.LastCommentDate, src => src.LastCommentDate)
                .Map(dest => dest.CommentsAreAllowed, src => src.CommentsAreAllowed)
                .Map(dest => dest.Comments, src => src.Comments);


            config.NewConfig<Comment, CommentForGettingDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Content, src => src.Content)
                .Map(dest => dest.CommentDate, src => src.CommentDate)
                .Map(dest => dest.ImageUrl, src => src.ImageUrl)
                .Map(dest => dest.AuthorOfComment, src => src.Author);

            config.NewConfig<CommentForCreatingDto, Comment>()
                .Map(dest => dest.Content, src => src.Content)
                .Map(dest => dest.TopicId, src => src.TopicId);

            config.NewConfig<CommentForUpdatingDto, Comment>()
                .Map(dest => dest.Content, src => src.Content)
                .Map(dest => dest.Id, src => src.Id);


            config.NewConfig<RegistrationRequestDto, ApplicationUser>()
                .Map(dest => dest.UserName, src => src.Email)
                .Map(dest => dest.NormalizedUserName, src => src.Email.ToUpper())
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.NormalizedEmail, src => src.Email.ToUpper())
                .Map(dest => dest.FullName, src => src.FullName)
                .Map(dest => dest.LockoutEnabled, src => true);

            config.NewConfig<ApplicationUser, AuthorForGettingDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.UserName, src => src.Email)
                .Map(dest => dest.FullName, src => src.FullName);
        }
    }
}
