using Forum.API.Application.DTO.Topics;
using Forum.Application.Contracts.Repository;
using Forum.Application.Contracts.Service;
using Forum.Application.Exceptions;
using Forum.Application.Features.Topics.Common;
using Forum.Application.Validators;
using Forum.Domain.Entities;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Forum.Application.Features.Topics.Commands.Handlers
{
    public class CreateTopicCommandHandler : TopicHelper, IRequestHandler<CreateTopicCommand, int>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IMapper _mapper;
        private readonly ICloudinaryImageService _cloudinaryImageService;
        private const int _width = 200;
        private const int _height = 200;

        public CreateTopicCommandHandler(
            ITopicRepository topicRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ICloudinaryImageService cloudinaryImageService) : base(httpContextAccessor)
        {
            _topicRepository = topicRepository;
            _mapper = mapper;
            _cloudinaryImageService = cloudinaryImageService;
        }

        public async Task<int> Handle(CreateTopicCommand request, CancellationToken cancellationToken)
        {
            var uploadResult = await _cloudinaryImageService.UploadAsync(request.model.Avatar, _width, _height, folder: "topics");

            var authenticatedUserId = AuthenticatedUserId();

            if (string.IsNullOrWhiteSpace(authenticatedUserId))
                throw new ForbidException(
                    Error.BuildErrorMessage("CreateTopicCommandHandler", "Unable to add topic for unauthorzied user")
                );

            var entity = _mapper.Map<Topic>(request.model);
            entity.AuthorId = authenticatedUserId;
            entity.ImageUrl = uploadResult.Url;
            entity.ImagePublicId = uploadResult.PublicId;

            try
            {
                await _topicRepository.AddAsync(entity);
                return await _topicRepository.SaveAsync();
            }
            catch
            {
                await _cloudinaryImageService.DeleteAsync(uploadResult.PublicId);
                throw;
            }
        }




        //#region VALIDATORS

        //private static void ValidateGuid(Guid id)
        //{
        //    if (id == Guid.Empty)
        //        throw new ArgumentException("Invalid id.");
        //}

        //private static void ValidateCreateModel(TopicForCreatingDto model)
        //{
        //    if (model == null)
        //        throw new ArgumentException("Request body is required.");

        //    if (string.IsNullOrWhiteSpace(model.Title))
        //        throw new ArgumentException("Title is required.");

        //    if (string.IsNullOrWhiteSpace(model.Content))
        //        throw new ArgumentException("Content is required.");

        //    if (model.Avatar is null || model.Avatar.Length == 0)
        //        throw new BadRequestException("Image is required parameter");
        //}

        //private static void ValidateUpdateModel(TopicForUpdatingDto model)
        //{
        //    if (model == null)
        //        throw new ArgumentException("Request body is required.");

        //    ValidateGuid(model.Id);

        //    if (string.IsNullOrWhiteSpace(model.Title))
        //        throw new ArgumentException("Title is required.");

        //    if (string.IsNullOrWhiteSpace(model.Content))
        //        throw new ArgumentException("Content is required.");
        //}

        //private static void ValidatePaging(int? pageNumber, int? pageSize)
        //{
        //    if (pageNumber <= 0)
        //        throw new ArgumentException("PageNumber must be greater than 0.");

        //    if (pageSize <= 0)
        //        throw new ArgumentException("PageSize must be greater than 0.");
        //}

        //#endregion


    }
}
