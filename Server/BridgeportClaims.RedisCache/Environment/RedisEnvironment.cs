using System;
using System.Collections.Specialized;
using System.Configuration;

namespace BridgeportClaims.RedisCache.Environment
{
    public class RedisEnvironment : IRedisEnvironment
    {
        public Lazy<NameValueCollection> RedisSettings { get; } = new Lazy<NameValueCollection>(() =>
            (NameValueCollection) ConfigurationManager.GetSection("redisSettings"));
    }
}