using Forum.API.Application.DTO.Comments;
using Forum.Application.Contracts.Repository;
using Forum.Application.Contracts.Service;
using Forum.Application.Exceptions;
using Forum.Domain.Entities;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Forum.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICloudinaryImageService _cloudinaryImageService;
        private const int _width = 200;
        private const int _height = 200;

        public CommentService(
            ICommentRepository commentRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ICloudinaryImageService cloudinaryImageService)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _cloudinaryImageService = cloudinaryImageService;
        }

        public async Task<int> AddNewCommentAsync(CommentForCreatingDto model)
        {
            ValidateCreateModel(model);

            var authenticatedUserId = AuthenticatedUserId();
            if (string.IsNullOrWhiteSpace(authenticatedUserId))
                throw new ForbidException("Unable to add a comment for unauthorzied user");

            var uploadResult = await _cloudinaryImageService.UploadAsync(model.Image, _width, _height, folder: "comments");

            var entity = _mapper.Map<Comment>(model);
            entity.AuthorId = authenticatedUserId;
            entity.ImageUrl = uploadResult.Url;
            entity.ImagePublicId = uploadResult.PublicId;

            try
            {
                await _commentRepository.AddAsync(entity);
                return await _commentRepository.SaveAsync();
            }
            catch
            {
                await _cloudinaryImageService.DeleteAsync(uploadResult.PublicId);
                throw;
            }
        }
        public async Task<int> DeleteCommentAsync(Guid commentId)
        {
            ValidateGuid(commentId);

            var comment = await _commentRepository.GetAsync(c => c.Id == commentId);

            if (comment == null)
                throw new ArgumentException($"Comment with id '{commentId}' not found.");

            if (!UserCanModifyContent(comment))
                throw new ForbidException($"Authenticated user have no permission");

            _commentRepository.Remove(comment);
            int result = await _commentRepository.SaveAsync();

            if (result > 0 && !string.IsNullOrWhiteSpace(comment.ImagePublicId))
                await _cloudinaryImageService.DeleteAsync(comment.ImagePublicId);

            return result;
        }
        public Task<int> UpdateCommentAsync(CommentForUpdatingDto model)
        {
            throw new NotImplementedException();
        }


        #region VALIDATORS
        private static void ValidateGuid(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Invalid id.");
        }
        private static void ValidateCreateModel(CommentForCreatingDto model)
        {
            if (string.IsNullOrWhiteSpace(model.Content))
                throw new BadRequestException("Comment content is required");

            if (model.TopicId == Guid.Empty)
                throw new BadRequestException("Topic id is required for comment to be added");
        }

        #endregion

        private bool UserCanModifyContent(Comment content)
        {
            string authenticatedUserId = AuthenticatedUserId();

            if (string.IsNullOrWhiteSpace(authenticatedUserId))
                return false;

            if (content.AuthorId.Trim() != authenticatedUserId.Trim())
                return false;

            return true;
        }
        private string AuthenticatedUserId()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true
                ? _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
                : string.Empty;
        }
    }
}
