using System;
using System.Collections.Specialized;

namespace BridgeportClaims.RedisCache.Environment
{
    public interface IRedisEnvironment
    {
        Lazy<NameValueCollection> RedisSettings { get; }
        bool IsProduction { get; }
    }
}