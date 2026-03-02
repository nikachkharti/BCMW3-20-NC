using Forum.Application.Contracts.Redis;
using StackExchange.Redis;
using System.Text.Json;

namespace Forum.Infrastructure.Redis
{
    public class RedisRepository<T> : IRedisRepository<T> where T : class
    {
        private readonly IDatabase _db;
        private readonly IConnectionMultiplexer _connection;
        private readonly JsonSerializerOptions _jsonOptions;

        public RedisRepository(IConnectionMultiplexer connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _db = connection.GetDatabase();
            _jsonOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        }

        // ── Basic CRUD ────────────────────────────────────────
        public async Task<bool> DeleteAsync(string key) => await _db.KeyDeleteAsync(key);
        public async Task<bool> ExistsAsync(string key) => await _db.KeyExistsAsync(key);
        public async Task<T> GetAsync(string key)
        {
            var value = await _db.StringGetAsync(key);
            return Deserialize(value);
        }
        public async Task<bool> SetAsync(string key, T value, TimeSpan? expiry = null)
        {
            var serialized = Serialize(value);
            if (expiry.HasValue)
                return await _db.StringSetAsync(key, serialized, expiry.Value, When.NotExists);
            return await _db.StringSetAsync(key, serialized, null, When.NotExists);
        }


        // ── Pattern Search ────────────────────────────────────
        public async Task<IEnumerable<string>> SearchKeysAsync(string pattern)
        {
            var keys = new List<string>();
            foreach (var endpoint in _connection.GetEndPoints())
            {
                var server = _connection.GetServer(endpoint);
                await foreach (var key in server.KeysAsync(pattern: pattern))
                    keys.Add(key!);
            }
            return keys;
        }
        public async Task<long> DeleteByPatternAsync(string pattern)
        {
            var keys = await SearchKeysAsync(pattern);
            var redisKeys = new List<RedisKey>();
            foreach (var k in keys) redisKeys.Add(k);
            return redisKeys.Count > 0
                ? await _db.KeyDeleteAsync(redisKeys.ToArray())
                : 0;
        }




        // ── Helpers ───────────────────────────────────────────
        private string Serialize(T value) => JsonSerializer.Serialize(value, _jsonOptions);
        private T Deserialize(RedisValue value) => value.IsNullOrEmpty ? null : JsonSerializer.Deserialize<T>((string)value!, _jsonOptions);
        private RedisValue[] SerializeMany(IEnumerable<T> values)
        {
            var list = new List<RedisValue>();
            foreach (var v in values) list.Add(Serialize(v));
            return list.ToArray();
        }


    }
}
