using System;
using System.Runtime.Caching;

namespace BridgeportClaims.Business.Caching
{
    public class CacheManager
    {
        private readonly MemoryCache _cache;
        private readonly object _semiphore = new object();

        public CacheManager(string key)
        {
            _cache = new MemoryCache(key);
        }

        private static CacheItemPolicy Policy => new CacheItemPolicy
        {
            AbsoluteExpiration = new DateTimeOffset(DateTime.UtcNow.AddDays(5))
        };

        public bool SetCache(string key, object value)
        {
            lock (_semiphore)
            {
                if (_cache.Contains(key))
                    _cache.Remove(key);
                return _cache.Add(key, value, Policy);
            }
        }
    }
}