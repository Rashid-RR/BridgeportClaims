using System;
using System.Runtime.Caching;

namespace BridgeportClaims.Common.Caching
{
    public class MemoryCacher : IMemoryCacher
    {
        public object GetValue(string key)
        {
            var memoryCache = MemoryCache.Default;
            return memoryCache.Get(key);
        }

        public bool Add(string key, object value, DateTimeOffset absExpiration)
        {
            var memoryCache = MemoryCache.Default;
            return memoryCache.Add(key, value, absExpiration);
        }

        public void Delete(string key)
        {
            var memoryCache = MemoryCache.Default;
            if (memoryCache.Contains(key))
                memoryCache.Remove(key);
        }
    }
}