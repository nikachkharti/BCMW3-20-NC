using Forum.API.Application.DTO.Topics;

namespace Forum.Application.Models.Redis.Topic
{
    public class TopicListCacheEntry
    {
        public List<TopicListForGettingDto> Topics { get; set; } = new();
        public int TotalCount { get; set; }
    }
}
