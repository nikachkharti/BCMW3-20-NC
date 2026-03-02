using StackExchange.Redis;

namespace Forum.Application.Contracts.Repository.Redis
{
    public interface IRedisRepository<T> where T : class
    {
        // Basic CRUD
        Task<T> GetAsync(string key);
        Task<bool> SetAsync(string key, T value, TimeSpan? expiry = null);
        Task<bool> DeleteAsync(string key);
        Task<bool> ExistsAsync(string key);


        // Expiry Management
        Task<bool> SetExpiryAsync(string key, TimeSpan expiry);
        Task<TimeSpan?> GetTimeToLiveAsync(string key);
        Task<bool> PersistAsync(string key); // Remove expiry


        // Bulk Operations
        Task<IEnumerable<T>> GetManyAsync(IEnumerable<string> keys);
        Task<bool> SetManyAsync(IDictionary<string, T> keyValuePairs, TimeSpan? expiry = null);
        Task<long> DeleteManyAsync(IEnumerable<string> keys);


        // Atomic / Conditional
        Task<bool> SetIfNotExistsAsync(string key, T value, TimeSpan? expiry = null); // SETNX
        Task<T> GetAndDeleteAsync(string key); // GETDEL
        Task<T> GetAndSetAsync(string key, T newValue);


        // Counter Operations (works when T is numeric / stored as string)
        Task<long> IncrementAsync(string key, long amount = 1);
        Task<double> IncrementByFloatAsync(string key, double amount);
        Task<long> DecrementAsync(string key, long amount = 1);


        // List Operations
        Task<long> ListPushLeftAsync(string key, T value);
        Task<long> ListPushRightAsync(string key, T value);
        Task<T> ListPopLeftAsync(string key);
        Task<T> ListPopRightAsync(string key);
        Task<IEnumerable<T>> ListRangeAsync(string key, long start = 0, long stop = -1);
        Task<long> ListLengthAsync(string key);


        // Set Operations
        Task<bool> SetAddAsync(string key, T value);
        Task<bool> SetRemoveAsync(string key, T value);
        Task<IEnumerable<T>> SetMembersAsync(string key);
        Task<bool> SetContainsAsync(string key, T value);
        Task<long> SetLengthAsync(string key);


        // Sorted Set (ZSet) Operations
        Task<bool> SortedSetAddAsync(string key, T value, double score);
        Task<IEnumerable<T>> SortedSetRangeByScoreAsync(string key, double min = double.NegativeInfinity, double max = double.PositiveInfinity);
        Task<IEnumerable<T>> SortedSetRangeByRankAsync(string key, long start = 0, long stop = -1);
        Task<double?> SortedSetScoreAsync(string key, T value);
        Task<bool> SortedSetRemoveAsync(string key, T value);
        Task<long> SortedSetLengthAsync(string key);


        // Hash Operations
        Task<bool> HashSetAsync(string key, string field, T value);
        Task<T> HashGetAsync(string key, string field);
        Task<bool> HashDeleteAsync(string key, string field);
        Task<bool> HashExistsAsync(string key, string field);
        Task<IDictionary<string, T>> HashGetAllAsync(string key);
        Task<IEnumerable<string>> HashKeysAsync(string key);


        // Pub/Sub
        Task PublishAsync(string channel, T message);
        Task SubscribeAsync(string channel, Action<T> handler);
        Task UnsubscribeAsync(string channel);


        // Pattern Search
        Task<IEnumerable<string>> SearchKeysAsync(string pattern);
        Task<long> DeleteByPatternAsync(string pattern);


        // Transactions
        Task<bool> ExecuteTransactionAsync(Func<ITransaction, Task> transactionBody);


        // Lua Scripting
        Task<RedisResult> ExecuteScriptAsync(string luaScript, string[] keys, params object[] args);


        // Pipeline / Batching
        Task ExecutePipelineAsync(Action<IBatch> batchActions);

    }
}
