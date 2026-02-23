using Forum.API.Application.DTO.Topics;
using MediatR;

namespace Forum.Application.Features.Topics.Commands
{
    public record UpdateTopicCommand(TopicForUpdatingDto model) : IRequest<int>;
}
