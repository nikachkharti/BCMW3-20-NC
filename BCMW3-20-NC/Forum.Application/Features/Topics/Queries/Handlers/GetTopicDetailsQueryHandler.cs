using Forum.API.Application.DTO.Topics;
using Forum.Application.Contracts.Repository;
using Forum.Application.Exceptions;
using Forum.Application.Validators;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Forum.Application.Features.Topics.Queries.Handlers
{
    public class GetTopicDetailsQueryHandler
        (ITopicRepository topicRepository, IMapper mapper)
        : IRequestHandler<GetTopicDetailsQuery, TopicDetailsForGettingDto>
    {
        public async Task<TopicDetailsForGettingDto> Handle(GetTopicDetailsQuery request, CancellationToken cancellationToken)
        {
            var topic = await topicRepository.GetAsync(
                t => t.Id == request.topcId,
                includes:
                    t => t.Include(t => t.Comments)
                        .ThenInclude(c => c.Author)
            );

            if (topic == null)
                throw new NotFoundException(Error.BuildErrorMessage("GetTopicDetailsQueryHandler", $"Topic with id '{request.topcId}' not found."));

            return mapper.Map<TopicDetailsForGettingDto>(topic);
        }
    }
}
