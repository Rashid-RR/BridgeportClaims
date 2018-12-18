using System;
using System.Collections.Generic;

namespace BridgeportClaims.RedisCache.Keys.Abstractions
{
    public abstract class AbstractCacheKey : ICacheKey
    {
        protected AbstractCacheKey()
        {
            CacheKey = "{{DefaultCacheKey_v1}}";
        }

        private const int TwentyFourHours = 24;
        public virtual string CacheKey { get; }
        public virtual TimeSpan RedisExpirationTimespan => new TimeSpan(0, TwentyFourHours, 0, 0);
        public virtual int LocalCacheAbsoluteExpirationMinutes => 5;
        public virtual int LocalCacheSlidingExpirationMinutes => 1;

        public override string ToString()
        {
            return CacheKey;
        }

        public override bool Equals(object obj)
        {
            return obj != null && ToString() == obj.ToString();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 101925009;
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CacheKey);
                return hashCode;
            }
        }
    }
}