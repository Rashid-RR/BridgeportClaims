using System;
using System.Collections.Specialized;
using System.Configuration;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.RedisCache.Environment
{
    public class RedisEnvironment : IRedisEnvironment
    {
        public Lazy<NameValueCollection> RedisSettings { get; } = new Lazy<NameValueCollection>(() =>
            (NameValueCollection) ConfigurationManager.GetSection("redisSettings"));

        public bool IsProduction { get; } = cs.IsProduction;
    }
}