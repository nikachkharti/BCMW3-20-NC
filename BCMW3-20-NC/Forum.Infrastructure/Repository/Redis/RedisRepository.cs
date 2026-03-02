using Forum.Application.Contracts.Repository.Redis;
using StackExchange.Redis;
using System.Text.Json;

namespace Forum.Infrastructure.Repository.Redis
{
    public class RedisRepository<T> : IRedisRepository<T> where T : class
    {
        private readonly IDatabase _db;
        private readonly ISubscriber _subscriber;
        private readonly IConnectionMultiplexer _connection;
        private readonly JsonSerializerOptions _jsonOptions;

        public RedisRepository(IConnectionMultiplexer connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _db = connection.GetDatabase();
            _subscriber = connection.GetSubscriber();
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        // ── Helpers ───────────────────────────────────────────
        private string Serialize(T value) =>
            JsonSerializer.Serialize(value, _jsonOptions);

        private T Deserialize(RedisValue value) =>
            value.IsNullOrEmpty ? null : JsonSerializer.Deserialize<T>((string)value!, _jsonOptions);

        private RedisValue[] SerializeMany(IEnumerable<T> values)
        {
            var list = new List<RedisValue>();
            foreach (var v in values) list.Add(Serialize(v));
            return list.ToArray();
        }

        // ── Basic CRUD ────────────────────────────────────────
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

        public async Task<bool> DeleteAsync(string key)
            => await _db.KeyDeleteAsync(key);

        public async Task<bool> ExistsAsync(string key)
            => await _db.KeyExistsAsync(key);

        // ── Expiry Management ─────────────────────────────────
        public async Task<bool> SetExpiryAsync(string key, TimeSpan expiry)
            => await _db.KeyExpireAsync(key, expiry);

        public async Task<TimeSpan?> GetTimeToLiveAsync(string key)
            => await _db.KeyTimeToLiveAsync(key);

        public async Task<bool> PersistAsync(string key)
            => await _db.KeyPersistAsync(key);

        // ── Bulk Operations ───────────────────────────────────
        public async Task<IEnumerable<T>> GetManyAsync(IEnumerable<string> keys)
        {
            var redisKeys = new List<RedisKey>();
            foreach (var k in keys) redisKeys.Add(k);

            var values = await _db.StringGetAsync(redisKeys.ToArray());
            var results = new List<T>();
            foreach (var v in values) results.Add(Deserialize(v));
            return results;
        }

        public async Task<bool> SetManyAsync(IDictionary<string, T> keyValuePairs, TimeSpan? expiry = null)
        {
            var entries = new List<KeyValuePair<RedisKey, RedisValue>>();
            foreach (var kvp in keyValuePairs)
                entries.Add(new KeyValuePair<RedisKey, RedisValue>(kvp.Key, Serialize(kvp.Value)));

            var success = await _db.StringSetAsync(entries.ToArray());

            if (success && expiry.HasValue)
            {
                var batch = _db.CreateBatch();
                foreach (var key in keyValuePairs.Keys)
                    _ = batch.KeyExpireAsync(key, expiry.Value);
                batch.Execute();
            }
            return success;
        }

        public async Task<long> DeleteManyAsync(IEnumerable<string> keys)
        {
            var redisKeys = new List<RedisKey>();
            foreach (var k in keys) redisKeys.Add(k);
            return await _db.KeyDeleteAsync(redisKeys.ToArray());
        }

        // ── Atomic / Conditional ──────────────────────────────
        public async Task<bool> SetIfNotExistsAsync(string key, T value, TimeSpan? expiry = null)
            => await _db.StringSetAsync(key, Serialize(value), expiry, When.NotExists);

        public async Task<T> GetAndDeleteAsync(string key)
        {
            var value = await _db.StringGetDeleteAsync(key);
            return Deserialize(value);
        }

        public async Task<T> GetAndSetAsync(string key, T newValue)
        {
            var old = await _db.StringGetSetAsync(key, Serialize(newValue));
            return Deserialize(old);
        }

        // ── Counter Operations ────────────────────────────────
        public async Task<long> IncrementAsync(string key, long amount = 1)
            => await _db.StringIncrementAsync(key, amount);

        public async Task<double> IncrementByFloatAsync(string key, double amount)
            => await _db.StringIncrementAsync(key, amount);

        public async Task<long> DecrementAsync(string key, long amount = 1)
            => await _db.StringDecrementAsync(key, amount);

        // ── List Operations ───────────────────────────────────
        public async Task<long> ListPushLeftAsync(string key, T value)
            => await _db.ListLeftPushAsync(key, Serialize(value));

        public async Task<long> ListPushRightAsync(string key, T value)
            => await _db.ListRightPushAsync(key, Serialize(value));

        public async Task<T> ListPopLeftAsync(string key)
        {
            var value = await _db.ListLeftPopAsync(key);
            return Deserialize(value);
        }

        public async Task<T> ListPopRightAsync(string key)
        {
            var value = await _db.ListRightPopAsync(key);
            return Deserialize(value);
        }

        public async Task<IEnumerable<T>> ListRangeAsync(string key, long start = 0, long stop = -1)
        {
            var values = await _db.ListRangeAsync(key, start, stop);
            var results = new List<T>();
            foreach (var v in values) results.Add(Deserialize(v));
            return results;
        }

        public async Task<long> ListLengthAsync(string key)
            => await _db.ListLengthAsync(key);

        // ── Set Operations ────────────────────────────────────
        public async Task<bool> SetAddAsync(string key, T value)
            => await _db.SetAddAsync(key, Serialize(value));

        public async Task<bool> SetRemoveAsync(string key, T value)
            => await _db.SetRemoveAsync(key, Serialize(value));

        public async Task<IEnumerable<T>> SetMembersAsync(string key)
        {
            var values = await _db.SetMembersAsync(key);
            var results = new List<T>();
            foreach (var v in values) results.Add(Deserialize(v));
            return results;
        }

        public async Task<bool> SetContainsAsync(string key, T value)
            => await _db.SetContainsAsync(key, Serialize(value));

        public async Task<long> SetLengthAsync(string key)
            => await _db.SetLengthAsync(key);

        // ── Sorted Set Operations ─────────────────────────────
        public async Task<bool> SortedSetAddAsync(string key, T value, double score)
            => await _db.SortedSetAddAsync(key, Serialize(value), score);

        public async Task<IEnumerable<T>> SortedSetRangeByScoreAsync(
            string key, double min = double.NegativeInfinity, double max = double.PositiveInfinity)
        {
            var values = await _db.SortedSetRangeByScoreAsync(key, min, max);
            var results = new List<T>();
            foreach (var v in values) results.Add(Deserialize(v));
            return results;
        }

        public async Task<IEnumerable<T>> SortedSetRangeByRankAsync(string key, long start = 0, long stop = -1)
        {
            var values = await _db.SortedSetRangeByRankAsync(key, start, stop);
            var results = new List<T>();
            foreach (var v in values) results.Add(Deserialize(v));
            return results;
        }

        public async Task<double?> SortedSetScoreAsync(string key, T value)
            => await _db.SortedSetScoreAsync(key, Serialize(value));

        public async Task<bool> SortedSetRemoveAsync(string key, T value)
            => await _db.SortedSetRemoveAsync(key, Serialize(value));

        public async Task<long> SortedSetLengthAsync(string key)
            => await _db.SortedSetLengthAsync(key);

        // ── Hash Operations ───────────────────────────────────
        public async Task<bool> HashSetAsync(string key, string field, T value)
        {
            await _db.HashSetAsync(key, field, Serialize(value));
            return true;
        }

        public async Task<T> HashGetAsync(string key, string field)
        {
            var value = await _db.HashGetAsync(key, field);
            return Deserialize(value);
        }

        public async Task<bool> HashDeleteAsync(string key, string field)
            => await _db.HashDeleteAsync(key, field);

        public async Task<bool> HashExistsAsync(string key, string field)
            => await _db.HashExistsAsync(key, field);

        public async Task<IDictionary<string, T>> HashGetAllAsync(string key)
        {
            var entries = await _db.HashGetAllAsync(key);
            var result = new Dictionary<string, T>();
            foreach (var entry in entries)
                result[entry.Name!] = Deserialize(entry.Value);
            return result;
        }

        public async Task<IEnumerable<string>> HashKeysAsync(string key)
        {
            var fields = await _db.HashKeysAsync(key);
            var result = new List<string>();
            foreach (var f in fields) result.Add(f!);
            return result;
        }

        // ── Pub/Sub ───────────────────────────────────────────
        public async Task PublishAsync(string channel, T message)
            => await _subscriber.PublishAsync(RedisChannel.Literal(channel), Serialize(message));

        public async Task SubscribeAsync(string channel, Action<T?> handler)
        {
            await _subscriber.SubscribeAsync(RedisChannel.Literal(channel), (_, value) =>
            {
                var deserialized = Deserialize(value);
                handler(deserialized);
            });
        }

        public async Task UnsubscribeAsync(string channel)
            => await _subscriber.UnsubscribeAsync(RedisChannel.Literal(channel));

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

        // ── Transactions ──────────────────────────────────────
        public async Task<bool> ExecuteTransactionAsync(Func<ITransaction, Task> transactionBody)
        {
            var transaction = _db.CreateTransaction();
            await transactionBody(transaction);
            return await transaction.ExecuteAsync();
        }

        // ── Lua Scripting ─────────────────────────────────────
        public async Task<RedisResult> ExecuteScriptAsync(string luaScript, string[] keys, object[] args)
        {
            var redisKeys = Array.ConvertAll(keys, k => (RedisKey)k);
            var redisArgs = Array.ConvertAll(args, a => (RedisValue)a.ToString()!);
            return await _db.ScriptEvaluateAsync(luaScript, redisKeys, redisArgs);
        }

        // ── Pipeline / Batching ───────────────────────────────
        public Task ExecutePipelineAsync(Action<IBatch> batchActions)
        {
            var batch = _db.CreateBatch();
            batchActions(batch);
            batch.Execute();
            return Task.CompletedTask;
        }

    }
}
