using System;
using StackExchange.Redis;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.RedisCache.Connection
{
    public static class RedisCacheConnectionHelper
    {
        public static ConnectionMultiplexer Connection => LazyConnection.Value;

        private static readonly Lazy<ConnectionMultiplexer> LazyConnection =
            new Lazy<ConnectionMultiplexer>(() =>
                ConnectionMultiplexer.Connect(cs.CacheConnection));
    }
}