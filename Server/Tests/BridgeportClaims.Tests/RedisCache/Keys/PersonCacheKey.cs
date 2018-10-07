using System;
using BridgeportClaims.RedisCache.Keys;
using BridgeportClaims.RedisCache.Keys.Abstractions;

namespace BridgeportClaims.Tests.RedisCache.Keys
{
    public class PersonCacheKey : AbstractCacheKey
    {
        private const int TenSeconds = 10;
        public const string KeyFormat = "{{PersonCacheKey_v1}}";
        public override TimeSpan RedisExpirationTimespan => new TimeSpan(0, 0, 0, TenSeconds);
        public override int LocalCacheAbsoluteExpirationMinutes => TenSeconds;
        public override string CacheKey => KeyFormat;
    }
}