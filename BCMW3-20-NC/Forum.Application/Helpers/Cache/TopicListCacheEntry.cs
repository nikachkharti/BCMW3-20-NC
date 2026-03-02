using Forum.API.Application.DTO.Topics;

namespace Forum.Application.Helpers.Cache
{
    public class TopicListCacheEntry
    {
        public List<TopicListForGettingDto> Topics { get; set; } = new();
        public int TotalCount { get; set; }
    }
}
