using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using BridgeportClaims.Common.Helpers;

namespace BridgeportClaims.Common.Caching
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class MemoryCacher : IMemoryCacher
    {
        private MemoryCache _memoryCache { get; } = MemoryCache.Default;
        private CacheItemPolicy _defaultPolicy { get; } = new CacheItemPolicy{AbsoluteExpiration = DateTimeOffset.UtcNow.AddDays(5)};

        public async Task<T> AddOrGetExisting<T>(string key, Func<Task<T>> valueFactory)
        {
            var asyncLazyValue = new AsyncLazy<T>(valueFactory);
            var existingValue = (AsyncLazy<T>) _memoryCache.AddOrGetExisting(key, asyncLazyValue, _defaultPolicy);

            if (null != existingValue)
                asyncLazyValue = existingValue;

            try
            {
                var result = await asyncLazyValue;

                // The awaited Task has completed. Check that the task still is the same version
                // that the cache returns (i.e. the awaited task has not been invalidated during the await).    
                if (asyncLazyValue != _memoryCache.AddOrGetExisting(key, new AsyncLazy<T>(valueFactory), _defaultPolicy))
                {
                    // The awaited value is no more the most recent one.
                    // Get the most recent value with a recursive call.
                    return await AddOrGetExisting(key, valueFactory);
                }
                return result;
            }
            catch (Exception)
            {
                // Task object for the given key failed with exception. Remove the task from the cache.
                _memoryCache.Remove(key);
                // Re throw the exception to be handled by the caller.
                throw;
            }
        }

        public T AddOrGetExisting<T>(string key, Func<T> valueFactory)
        {
            var newValue = new Lazy<T>(valueFactory);
            var oldValue = _memoryCache.AddOrGetExisting(key, newValue, _defaultPolicy) as Lazy<T>;

            try
            {
                return (oldValue ?? newValue).Value;
            }
            catch
            {
                DeleteIfExists(key);
                throw;
            }
        }

        public void DeleteIfExists(string key)
        {
            if (_memoryCache.Contains(key))
                _memoryCache.Remove(key);
        }

        public bool Contains(string key) => _memoryCache.Contains(key);

        public void DeleteAll()
        {
            // A snapshot of keys is taken to avoid enumerating collection during changes.
            var keys = _memoryCache.Select(c => c.Key).ToList();
            keys.ForEach(DeleteIfExists);
        }
    }
}
