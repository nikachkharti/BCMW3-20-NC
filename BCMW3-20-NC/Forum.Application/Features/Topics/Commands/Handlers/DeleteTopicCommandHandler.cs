using Forum.Application.Contracts.Repository;
using Forum.Application.Contracts.Service;
using Forum.Application.Exceptions;
using Forum.Application.Features.Topics.Common;
using Forum.Application.Validators;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Forum.Application.Features.Topics.Commands.Handlers
{
    public class DeleteTopicCommandHandler : TopicHelper, IRequestHandler<DeleteTopicCommand, int>
    {
        private readonly ICloudinaryImageService _cloudinaryImageService;
        private readonly ITopicRepository _topicRepository;

        public DeleteTopicCommandHandler(
            IHttpContextAccessor httpContextAccessor,
            ICloudinaryImageService cloudinaryImageService,
            ITopicRepository topicRepository
            ) : base(httpContextAccessor)
        {
            _cloudinaryImageService = cloudinaryImageService;
            _topicRepository = topicRepository;
        }

        public async Task<int> Handle(DeleteTopicCommand request, CancellationToken cancellationToken)
        {
            var topic = await _topicRepository.GetAsync(t => t.Id == request.Id);

            if (topic == null)
                throw new BadRequestException(Error.BuildErrorMessage("DeleteTopicCommandHandler",
                    $"Topic with id '{request.Id}' not found"));

            if (!UserCanModifyContent(topic))
                throw new ForbidException(Error.BuildErrorMessage("DeleteTopicAsync", "Authenticated user have no permission"));

            _topicRepository.Remove(topic);

            int result = await _topicRepository.SaveAsync();

            if (result > 0 && !string.IsNullOrWhiteSpace(topic.ImagePublicId))
                await _cloudinaryImageService.DeleteAsync(topic.ImagePublicId);

            return result;
        }
    }
}
