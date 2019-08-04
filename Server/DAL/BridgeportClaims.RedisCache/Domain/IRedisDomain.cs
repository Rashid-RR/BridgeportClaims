using System;
using System.Threading.Tasks;
using BridgeportClaims.RedisCache.Keys.Abstractions;
using BridgeportClaims.RedisCache.Redis;

namespace BridgeportClaims.RedisCache.Domain
{
    public interface IRedisDomain
    {
        Task<bool> AddAsync<T>(ICacheKey key, T value, TimeSpan expirationTime) where T : class;
        Task<IRedisResult<T>> GetAsync<T>(ICacheKey key) where T : class;
        Task<bool> RemoveAsync(ICacheKey key);
        Task<bool> SetKeyExpirationAsync(ICacheKey key, TimeSpan expirationTime);
        Task<bool> KeyExists(ICacheKey key);
    }
}