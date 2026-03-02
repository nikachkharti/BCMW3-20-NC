using Forum.API.Application.DTO.Topics;
using Forum.Application.Contracts.Repository;
using Forum.Application.Contracts.Repository.Redis;
using Forum.Application.Contracts.Service;
using Forum.Application.Exceptions;
using Forum.Application.Helpers.Cache;
using Forum.Application.Models.DTO.Comments;
using Forum.Domain.Entities;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;

namespace Forum.Application.Services
{
    public class TopicService : ITopicService
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICloudinaryImageService _cloudinaryImageService;
        private readonly IRedisRepository<TopicDetailsForGettingDto> _detailsCache;
        private readonly IRedisRepository<TopicListCacheEntry> _listCache;
        private const int _width = 200;
        private const int _height = 200;


        public TopicService(
            ITopicRepository topicRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IRedisRepository<TopicDetailsForGettingDto> detailsCache,
            IRedisRepository<TopicListCacheEntry> listCache,
            ICloudinaryImageService cloudinaryImageService)
        {
            _topicRepository = topicRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _cloudinaryImageService = cloudinaryImageService;
            _detailsCache = detailsCache;
            _listCache = listCache;
        }


        // ─────────────────────────────────────────────────────────────────────
        // ADD NEW TOPIC
        //
        // Strategy: WRITE-AROUND + CACHE INVALIDATION
        //
        // We intentionally do NOT write the new topic to cache on creation.
        // Instead we invalidate all list cache entries so they are reloaded
        // fresh from DB on next request. The newly created topic's detail
        // cache will be populated on first read (Cache-Aside).
        //
        // Why not Write-Through here?
        //   The list cache contains pagination metadata (TotalCount, ordering)
        //   that would require re-computing multiple page entries. It is
        //   simpler and safer to invalidate and let Cache-Aside repopulate.
        // ─────────────────────────────────────────────────────────────────────
        public async Task<int> AddNewTopicAsync(TopicForCreatingDto model)
        {
            ValidateCreateModel(model);

            var uploadResult = await _cloudinaryImageService.UploadAsync(model.Avatar, _width, _height, folder: "topics");
            var authenticatedUserId = AuthenticatedUserId();

            if (string.IsNullOrWhiteSpace(authenticatedUserId))
                throw new ForbidException("Unable to add topic for unauthorized user");

            var entity = _mapper.Map<Topic>(model);
            entity.AuthorId = authenticatedUserId;
            entity.ImageUrl = uploadResult.Url;
            entity.ImagePublicId = uploadResult.PublicId;

            try
            {
                await _topicRepository.AddAsync(entity);
                int result = await _topicRepository.SaveAsync();

                // Invalidate all list pages — new topic changes counts and order
                await InvalidateAllListCacheAsync();

                return result;
            }
            catch
            {
                await _cloudinaryImageService.DeleteAsync(uploadResult.PublicId);
                throw;
            }
        }


        // ─────────────────────────────────────────────────────────────────────
        // DELETE TOPIC
        //
        // Strategy: CACHE INVALIDATION
        //
        // After successful deletion, remove the detail entry for this topic
        // and wipe all list cache pages.
        // ─────────────────────────────────────────────────────────────────────
        public async Task<int> DeleteTopicAsync(Guid topicId)
        {
            ValidateGuid(topicId);

            var topic = await _topicRepository.GetAsync(t => t.Id == topicId);

            if (topic == null)
                throw new ArgumentException($"Topic with id '{topicId}' not found.");

            if (!UserCanModifyContent(topic))
                throw new ForbidException("Authenticated user has no permission");

            _topicRepository.Remove(topic);
            int result = await _topicRepository.SaveAsync();

            if (result > 0)
            {
                // Invalidate detail cache for this specific topic
                await SafeCacheDeleteAsync(() => _detailsCache.DeleteAsync(TopicCacheKeys.Details(topicId)));

                // Invalidate all list pages — topic no longer exists
                await InvalidateAllListCacheAsync();

                // Delete image from Cloudinary after successful DB + cache cleanup
                if (!string.IsNullOrWhiteSpace(topic.ImagePublicId))
                    await _cloudinaryImageService.DeleteAsync(topic.ImagePublicId);
            }

            return result;
        }


        // ─────────────────────────────────────────────────────────────────────
        // GET ALL TOPICS (PAGINATED)
        //
        // Strategy: CACHE-ASIDE per page
        //
        // Each unique page+size combination gets its own cache entry.
        // Example:
        //   "topics:list:page:1:size:10"
        //   "topics:list:page:2:size:10"
        //
        // Why short TTL (5 min) for lists?
        //   Lists change whenever ANY topic is added or deleted. A short TTL
        //   limits stale data without requiring complex invalidation across
        //   all page keys. If immediate consistency is critical, call
        //   InvalidateAllListCacheAsync() on every write instead.
        // ─────────────────────────────────────────────────────────────────────
        public async Task<(List<TopicListForGettingDto> Topics, int TotalCount)> GetAllTopicsAsync(
            int? pageNumber,
            int? pageSize)
        {
            ValidatePaging(pageNumber, pageSize);

            var cacheKey = TopicCacheKeys.List(pageNumber, pageSize);

            // 1. Try cache first
            var cached = await SafeCacheGetAsync(() => _listCache.GetAsync(cacheKey));
            if (cached != null)
            {
                Log.Information("Cache HIT for topic list. Key: {Key}", cacheKey);
                return (cached.Topics, cached.TotalCount);
            }

            Log.Information("Cache MISS for topic list. Key: {Key} — querying DB", cacheKey);

            // 2. Cache miss — query the database
            var result = await _topicRepository.GetAllAsync(
                pageNumber: pageNumber,
                pageSize: pageSize,
                orderBy: "CreateDate",
                ascending: false
            );

            var topics = _mapper.Map<List<TopicListForGettingDto>>(result.Items) ?? new List<TopicListForGettingDto>();

            // 3. Wrap list + count together in a single cache entry
            var entry = new TopicListCacheEntry
            {
                Topics = topics,
                TotalCount = result.TotalCount
            };

            await SafeCacheSetAsync(() => _listCache.SetAsync(cacheKey, entry, TopicCacheTtl.List));

            return (topics, result.TotalCount);
        }



        // ─────────────────────────────────────────────────────────────────────
        // GET TOPIC DETAILS
        //
        // Strategy: CACHE-ASIDE (Lazy Loading)
        //
        // 1. Check Redis first.
        // 2. On cache HIT  → return immediately (no DB query).
        // 3. On cache MISS → query DB, store result in Redis, then return.
        //
        // Why Cache-Aside for details?
        //   Topic details (with comments) are expensive to load due to JOINs.
        //   They are read far more often than they are written, making them
        //   a perfect candidate for caching.
        // ─────────────────────────────────────────────────────────────────────
        public async Task<TopicDetailsForGettingDto> GetTopicDetailsAsync(Guid topicId)
        {
            ValidateGuid(topicId);

            var cacheKey = TopicCacheKeys.Details(topicId);

            // 1. Try cache first
            var cached = await SafeCacheGetAsync(() => _detailsCache.GetAsync(cacheKey));
            if (cached != null)
            {
                Log.Information("Cache HIT for topic details. Key: {Key}", cacheKey);
                return cached;
            }

            Log.Information("Cache MISS for topic details. Key: {Key} — querying DB", cacheKey);

            // 2. Cache miss — query the database
            var topic = await _topicRepository.GetAsync(
                t => t.Id == topicId,
                includes: t => t.Include(t => t.Comments)
                                .ThenInclude(c => c.Author)
            );

            if (topic == null)
                throw new ArgumentException($"Topic with id '{topicId}' not found.");

            var dto = _mapper.Map<TopicDetailsForGettingDto>(topic);

            // 3. Populate cache for next request
            await SafeCacheSetAsync(() => _detailsCache.SetAsync(cacheKey, dto, TopicCacheTtl.Details));

            return dto;
        }



        // ─────────────────────────────────────────────────────────────────────
        // UPDATE TOPIC
        //
        // Strategy: CACHE INVALIDATION (Invalidate on Write)
        //
        // After a successful DB update we delete the specific topic's detail
        // cache entry AND all list cache entries (title may appear in lists).
        //
        // Why not update the cache in place (Write-Through)?
        //   The mapper mutates the tracked EF entity. We'd need to re-map
        //   after save to get the final state. Invalidating and letting
        //   Cache-Aside repopulate on next read is simpler and avoids
        //   serving a partially-updated cached object.
        // ─────────────────────────────────────────────────────────────────────
        public async Task<int> UpdateTopicAsync(TopicForUpdatingDto model)
        {
            ValidateUpdateModel(model);

            var topic = await _topicRepository.GetAsync(t => t.Id == model.Id);

            if (topic == null)
                throw new ArgumentException($"Topic with id '{model.Id}' not found.");

            if (!UserCanModifyContent(topic))
                throw new ForbidException("Authenticated user has no permission");

            _mapper.Map(model, topic);
            int result = await _topicRepository.SaveAsync();

            if (result > 0)
            {
                // Invalidate the specific topic details entry
                await SafeCacheDeleteAsync(() => _detailsCache.DeleteAsync(TopicCacheKeys.Details(model.Id)));

                // Invalidate all list pages (title shown in lists may have changed)
                await InvalidateAllListCacheAsync();
            }

            return result;
        }


        #region HELPERS

        private bool UserCanModifyContent(Topic content)
        {
            string authenticatedUserId = AuthenticatedUserId();

            if (string.IsNullOrWhiteSpace(authenticatedUserId))
                return false;

            if (content.AuthorId.Trim() != authenticatedUserId.Trim())
                return false;

            return true;
        }

        private string AuthenticatedUserId() =>
            _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true
                ? _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
                : string.Empty;

        // ─────────────────────────────────────────────────────────────────────
        // CACHE HELPERS
        // All Redis calls are wrapped in try/catch.
        //
        // IMPORTANT — Cache should NEVER break the application.
        // If Redis is down or throws, we log the error and fall through to
        // the database. This pattern is called "Cache-Aside with Resilience".
        // ─────────────────────────────────────────────────────────────────────

        private static async Task<TResult> SafeCacheGetAsync<TResult>(Func<Task<TResult>> cacheCall) where TResult : class
        {
            try { return await cacheCall(); }
            catch (Exception ex)
            {
                Log.Information(ex, "Redis GET failed — falling through to database");
                return null;
            }
        }

        private static async Task SafeCacheSetAsync(Func<Task<bool>> cacheCall)
        {
            try { await cacheCall(); }
            catch (Exception ex) { Log.Warning(ex, "Redis SET failed — cache not populated"); }
        }

        private static async Task SafeCacheDeleteAsync(Func<Task<bool>> cacheCall)
        {
            try { await cacheCall(); }
            catch (Exception ex) { Log.Warning(ex, "Redis DELETE failed — stale data may exist"); }
        }

        private async Task InvalidateAllListCacheAsync()
        {
            try { await _listCache.DeleteByPatternAsync(TopicCacheKeys.AllTopicsPattern); }
            catch (Exception ex) { Log.Warning(ex, "Redis pattern DELETE failed — list cache may be stale"); }
        }


        #endregion


        #region VALIDATORS

        private static void ValidateGuid(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Invalid id.");
        }

        private static void ValidateCreateModel(TopicForCreatingDto model)
        {
            if (model == null)
                throw new ArgumentException("Request body is required.");

            if (string.IsNullOrWhiteSpace(model.Title))
                throw new ArgumentException("Title is required.");

            if (string.IsNullOrWhiteSpace(model.Content))
                throw new ArgumentException("Content is required.");

            if (model.Avatar is null || model.Avatar.Length == 0)
                throw new BadRequestException("Image is required parameter");
        }

        private static void ValidateUpdateModel(TopicForUpdatingDto model)
        {
            if (model == null)
                throw new ArgumentException("Request body is required.");

            ValidateGuid(model.Id);

            if (string.IsNullOrWhiteSpace(model.Title))
                throw new ArgumentException("Title is required.");

            if (string.IsNullOrWhiteSpace(model.Content))
                throw new ArgumentException("Content is required.");
        }

        private static void ValidatePaging(int? pageNumber, int? pageSize)
        {
            if (pageNumber <= 0)
                throw new ArgumentException("PageNumber must be greater than 0.");

            if (pageSize <= 0)
                throw new ArgumentException("PageSize must be greater than 0.");
        }

        #endregion
    }
}
