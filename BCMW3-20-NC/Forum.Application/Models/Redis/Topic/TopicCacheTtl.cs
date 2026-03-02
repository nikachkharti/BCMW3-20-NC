namespace Forum.Application.Models.Redis.Topic
{
    // ─────────────────────────────────────────────────────────────────────────
    // CACHE TTL CONSTANTS
    // Different data has different freshness requirements.
    // ─────────────────────────────────────────────────────────────────────────
    public static class TopicCacheTtl
    {
        // Topic details (title, content, comments) — moderately volatile
        public static readonly TimeSpan Details = TimeSpan.FromMinutes(30);

        // Paginated list — changes whenever any topic is added/deleted
        public static readonly TimeSpan List = TimeSpan.FromMinutes(5);
    }
}
