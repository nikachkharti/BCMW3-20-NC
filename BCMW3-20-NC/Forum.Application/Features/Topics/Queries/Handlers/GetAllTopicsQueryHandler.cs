using Forum.API.Application.DTO.Topics;
using Forum.Application.Contracts.Repository;
using MapsterMapper;
using MediatR;

namespace Forum.Application.Features.Topics.Queries.Handlers
{
    public class GetAllTopicsQueryHandler
        (ITopicRepository topicRepository, IMapper mapper)
        : IRequestHandler<GetAllTopicsQuery, (List<TopicListForGettingDto> Topics, int TotalCount)>
    {

        public async Task<(List<TopicListForGettingDto> Topics, int TotalCount)> Handle(GetAllTopicsQuery request, CancellationToken cancellationToken)
        {
            var result = await topicRepository.GetAllAsync(
                pageNumber: request.pageNumber,
                pageSize: request.pageSize,
                orderBy: "CreateDate",
                ascending: false
            );

            var topics = mapper.Map<List<TopicListForGettingDto>>(result.Items);
            return (topics, result.TotalCount);
        }
    }
}
