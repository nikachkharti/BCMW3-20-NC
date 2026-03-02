namespace Forum.Application.Helpers.Cache
{
    // ─────────────────────────────────────────────────────────────────────────
    // CACHE KEY CONSTANTS
    // Centralizing all key names prevents typos and makes invalidation easy.
    // Pattern used: "{entity}:{identifier}:{dataType}"
    // ─────────────────────────────────────────────────────────────────────────
    public static class TopicCacheKeys
    {
        // Single topic details  →  "topic:3fa85f64-...:details"
        public static string Details(Guid id) => $"topic:{id}:details";

        // Paginated list page   →  "topics:list:page:1:size:10"
        public static string List(int? page, int? size) => $"topics:list:page:{page ?? 1}:size:{size ?? 10}";

        // Pattern to wipe ALL topic cache entries at once
        public const string AllTopicsPattern = "topic*";
    }
}
