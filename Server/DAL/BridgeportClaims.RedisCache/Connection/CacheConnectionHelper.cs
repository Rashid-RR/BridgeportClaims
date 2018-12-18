using System;
using StackExchange.Redis;
using cs = BridgeportClaims.Common.Config.ConfigService;
using s = BridgeportClaims.Common.Constants.StringConstants;

namespace BridgeportClaims.RedisCache.Connection
{
    public static class CacheConnectionHelper
    {
        public static ConnectionMultiplexer Connection => LazyConnection.Value;

        private static readonly Lazy<ConnectionMultiplexer> LazyConnection =
            new Lazy<ConnectionMultiplexer>(() =>
                ConnectionMultiplexer.Connect(cs.GetAppSetting(s.RedisCacheConnection)));
    }
}