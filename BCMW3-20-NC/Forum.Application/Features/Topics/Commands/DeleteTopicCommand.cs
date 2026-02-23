using MediatR;

namespace Forum.Application.Features.Topics.Commands
{
    public record DeleteTopicCommand(Guid Id) : IRequest<int>;
}
