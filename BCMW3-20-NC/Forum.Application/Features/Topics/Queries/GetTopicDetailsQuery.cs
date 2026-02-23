using Forum.API.Application.DTO.Topics;
using MediatR;

namespace Forum.Application.Features.Topics.Queries
{
    public record GetTopicDetailsQuery(Guid topcId) : IRequest<TopicDetailsForGettingDto>;
}
