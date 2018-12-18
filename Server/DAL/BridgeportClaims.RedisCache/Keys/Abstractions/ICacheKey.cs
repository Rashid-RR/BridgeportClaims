using System;

namespace BridgeportClaims.RedisCache.Keys.Abstractions
{
    public interface ICacheKey
    {
        string CacheKey { get; }
        TimeSpan RedisExpirationTimespan { get; }
        int LocalCacheAbsoluteExpirationMinutes { get; }
        int LocalCacheSlidingExpirationMinutes { get; }
    }
}