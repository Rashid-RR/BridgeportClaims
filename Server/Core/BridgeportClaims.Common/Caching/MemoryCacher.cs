﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using BridgeportClaims.Common.Helpers;

namespace BridgeportClaims.Common.Caching
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class MemoryCacher : CachingProviderBase, IMemoryCacher
    {
        #region Singleton

        private static readonly Lazy<MemoryCacher> Lazy =
            new Lazy<MemoryCacher>(() => new MemoryCacher());

        public static MemoryCacher Instance => Lazy.Value;

        #endregion

        #region ICachingProvider

        public string GetPaymentPostingCacheKey(string userId) => $"__PaymentPosting__{userId}__";

        public new virtual void UpdateItem(string key, object value)
        {
            base.UpdateItem(key, value);
        }

        #endregion

        private MemoryCache _cache { get; } = MemoryCache.Default;
        private CacheItemPolicy _defaultPolicy { get; } = new CacheItemPolicy();

        public async Task<T> AddOrGetExisting<T>(string key, Func<Task<T>> valueFactory)
        {

            var asyncLazyValue = new AsyncLazy<T>(valueFactory);
            var existingValue = (AsyncLazy<T>)_cache.AddOrGetExisting(key, asyncLazyValue, _defaultPolicy);

            if (existingValue != null)
            {
                asyncLazyValue = existingValue;
            }

            try
            {
                var result = await asyncLazyValue;

                // The awaited Task has completed. Check that the task still is the same version
                // that the cache returns (i.e. the awaited task has not been invalidated during the await).    
                if (asyncLazyValue != _cache.AddOrGetExisting(key, new AsyncLazy<T>(valueFactory), _defaultPolicy))
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
                _cache.Remove(key);
                // Re throw the exception to be handled by the caller.
                throw;
            }
        }

        public T AddOrGetExisting<T>(string key, Func<T> valueFactory)
        {
            var newValue = new Lazy<T>(valueFactory);
            var oldValue = _cache.AddOrGetExisting(key, newValue, _defaultPolicy) as Lazy<T>;

            try
            {
                return (oldValue ?? newValue).Value;
            }
            catch
            {
                Delete(key);
                throw;
            }
        }

        /// <summary>
        /// Works if you use AddOrGetExisting
        /// </summary>
        /// <param name="key"></param>
        public void Delete(string key) => _cache.Remove(key);
        

        /// <summary>
        /// Works if you use AddOrGetExisting
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(string key) => _cache.Contains(key);

        /// <summary>
        /// Works either way.
        /// </summary>
        public void DeleteAll()
        {
            // A snapshot of keys is taken to avoid enumerating collection during changes.
            var keys = _cache.Select(c => c.Key).ToList();
            keys.ForEach(Delete);
        }
    }
}
