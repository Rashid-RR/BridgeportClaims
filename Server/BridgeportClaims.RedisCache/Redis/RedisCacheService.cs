using System;
using StackExchange.Redis;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.RedisCache.Redis
{
    public class RedisCacheService
    {
        private readonly Lazy<ConnectionMultiplexer> _lazyConnection;
        private readonly IDatabase _cache;

        public RedisCacheService()
        {
            _lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                var cacheConn = cs.GetRedisCacheConnStr();
                return ConnectionMultiplexer.Connect(cacheConn);
            });
            _cache = _lazyConnection.Value.GetDatabase();
        }
    }
}