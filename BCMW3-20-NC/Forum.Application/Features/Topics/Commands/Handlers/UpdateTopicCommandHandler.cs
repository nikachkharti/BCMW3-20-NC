using Forum.Application.Contracts.Repository;
using Forum.Application.Exceptions;
using Forum.Application.Features.Topics.Common;
using Forum.Application.Validators;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Forum.Application.Features.Topics.Commands.Handlers
{
    public class UpdateTopicCommandHandler : TopicHelper, IRequestHandler<UpdateTopicCommand, int>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IMapper _mapper;

        public UpdateTopicCommandHandler(
            ITopicRepository topicRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _topicRepository = topicRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(UpdateTopicCommand request, CancellationToken cancellationToken)
        {
            var topic = await _topicRepository.GetAsync(t => t.Id == request.model.Id);

            if (topic == null)
                throw new BadRequestException(Error.BuildErrorMessage("UpdateTopicCommandHandler", $"Topic with id '{request.model.Id}' not found"));

            if (!UserCanModifyContent(topic))
                throw new ForbidException(Error.BuildErrorMessage("UpdateTopicAsync", "Authenticated user have no permission"));

            _mapper.Map(request.model, topic);
            return await _topicRepository.SaveAsync();
        }
    }
}
