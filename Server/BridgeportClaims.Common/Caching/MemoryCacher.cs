using System;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using BridgeportClaims.Common.Helpers;

namespace BridgeportClaims.Common.Caching
{
    public class MemoryCacher : CachingProviderBase, IMemoryCacher
    {
        #region Singleton 
        private static readonly Lazy<MemoryCacher> Lazy =
            new Lazy<MemoryCacher>(() => new MemoryCacher());

        public static MemoryCacher Instance => Lazy.Value;

        private MemoryCacher() { }

        #endregion

        #region ICachingProvider

        public string GetPaymentPostingCacheKey(string userId) => $"__PaymentPosting__{userId}__";

        public new virtual void AddItem(string key, object value)
        {
            base.AddItem(key, value);
        }

        public virtual object GetItem(string key)
        {
            return base.GetItem(key, true);//Remove default is true because it's Global Cache!
        }

        public new virtual object GetItem(string key, bool remove)
        {
            return base.GetItem(key, remove);
        }

        #endregion

        private MemoryCache MemoryCache { get; } = MemoryCache.Default;
        private CacheItemPolicy DefaultPolicy { get; } = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.UtcNow.AddDays(5) };

        public async Task<T> AddOrGetExisting<T>(string key, Func<Task<T>> valueFactory, CacheItemPolicy policy = null)
        {
            var useThisPolicy = policy ?? DefaultPolicy;
            var asyncLazyValue = new AsyncLazy<T>(valueFactory);
            var existingValue = (AsyncLazy<T>)MemoryCache.AddOrGetExisting(key, asyncLazyValue, useThisPolicy);

            if (null != existingValue)
                asyncLazyValue = existingValue;

            try
            {
                var result = await asyncLazyValue;

                // The awaited Task has completed. Check that the task still is the same version
                // that the cache returns (i.e. the awaited task has not been invalidated during the await).    
                if (asyncLazyValue != MemoryCache.AddOrGetExisting(key, new AsyncLazy<T>(valueFactory), DefaultPolicy))
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
                MemoryCache.Remove(key);
                // Re throw the exception to be handled by the caller.
                throw;
            }
        }

        public T AddOrGetExisting<T>(string key, Func<T> valueFactory, CacheItemPolicy policy = null)
        {
            var useThisPolicy = policy ?? DefaultPolicy;
            var newValue = new Lazy<T>(valueFactory);
            var oldValue = MemoryCache.AddOrGetExisting(key, newValue, useThisPolicy) as Lazy<T>;

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
            if (MemoryCache.Contains(key))
                MemoryCache.Remove(key);
        }

        public bool Contains(string key) => MemoryCache.Contains(key);

        public void DeleteAll()
        {
            // A snapshot of keys is taken to avoid enumerating collection during changes.
            var keys = MemoryCache.Select(c => c.Key).ToList();
            keys.ForEach(DeleteIfExists);
        }
    }
}
