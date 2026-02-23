using Forum.API.Application.DTO.Topics;
using MediatR;

namespace Forum.Application.Features.Topics.Queries
{
    public record GetAllTopicsQuery(int? pageNumber, int? pageSize) : IRequest<(List<TopicListForGettingDto> Topics, int TotalCount)>;
}
