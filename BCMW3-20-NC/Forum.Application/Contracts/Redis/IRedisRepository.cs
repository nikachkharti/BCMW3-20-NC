namespace Forum.Application.Contracts.Redis
{
    public interface IRedisRepository<T> where T : class
    {
        //Basic CRUD
        Task<T> GetAsync(string key);
        Task<bool> SetAsync(string key, T value, TimeSpan? expiry = null);
        Task<bool> DeleteAsync(string key);
        Task<bool> ExistsAsync(string key);


        // Pattern Search
        Task<IEnumerable<string>> SearchKeysAsync(string pattern);
        Task<long> DeleteByPatternAsync(string pattern);
    }
}
