using System;
using System.Threading.Tasks;
using BridgeportClaims.RedisCache.Keys;
using BridgeportClaims.RedisCache.Redis;

namespace BridgeportClaims.RedisCache.Domain
{
    public interface IRedisDomain
    {
        Task<bool> AddAsync<T>(ICacheKey key, T value, TimeSpan expirationTime)
            where T : class;

        Task<IRedisResult<T>> GetAsync<T>(ICacheKey key)
            where T : class;

        Task<bool> RemoveAsync(ICacheKey key);

        /*Task<byte[]> SerializeObjectAsync<T>(T obj);
        Task<T> DeserializeObjectAsync<T>(byte[] objData);
        Task<bool> AddHashEntryAsync<T>(ICacheKey key, KeyValuePair<ICacheKey, T> entry);
        Task<bool> AddHashEntriesAsync<T>(ICacheKey key, Dictionary<ICacheKey, T> entries);
        Task<IRedisResult<T>> GetHashEntryAsync<T>(ICacheKey key, ICacheKey name);
        Task<Dictionary<string, IRedisResult<T>>> GetHashEntryAsync<T>(ICacheKey key);
        Task RemoveHashEntryAsync(ICacheKey key, ICacheKey name);
        Task<bool> RemoveHashAsync(ICacheKey key);
        Task<Dictionary<ICacheKey, IRedisResult<T>>> GetMultipleAsync<T>(IEnumerable<ICacheKey> keys);
        Task<bool> SetKeyExpirationAsync(ICacheKey key, TimeSpan expirationTime);*/
    }
}