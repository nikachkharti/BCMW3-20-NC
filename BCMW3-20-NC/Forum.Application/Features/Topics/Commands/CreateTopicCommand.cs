using Forum.API.Application.DTO.Topics;
using MediatR;

namespace Forum.Application.Features.Topics.Commands
{
    public record CreateTopicCommand(TopicForCreatingDto model) : IRequest<int>;
}