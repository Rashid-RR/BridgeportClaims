using System;
using BridgeportClaims.RedisCache.Keys.Abstractions;

namespace BridgeportClaims.RedisCache.Keys
{
    public class ClaimUserHistoryCacheKey : AbstractCacheKey
    {
        private const int SevenHours = 7;

        #region Ctor

        public ClaimUserHistoryCacheKey(string userId)
        {
            UserIdKey = userId;
        }

        #endregion

        public const string KeyFormat = "{{ClaimUserHistoryCacheKey_v1}}|{0}";
        public string UserIdKey { get; set; }
        public override TimeSpan RedisExpirationTimespan => new TimeSpan(0, SevenHours, 0, 0);
        public override int LocalCacheAbsoluteExpirationMinutes => 15;
        public override string CacheKey => string.Format(KeyFormat, UserIdKey);
    }
}