using NLog;
using System;
using System.Runtime.Caching;
using cs = BridgeportClaims.Common.Config.ConfigService;
using c = BridgeportClaims.Common.StringConstants.Constants;

namespace BridgeportClaims.Common.Caching
{
    public abstract class CachingProviderBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected MemoryCache Cache = new MemoryCache(c.CachingProvider);
        private static readonly object Semaphore  = new object();
        private readonly DateTimeOffset _defaultExpiry;

        protected CachingProviderBase()
        {
            _defaultExpiry = DateTimeOffset.UtcNow.AddDays(14);
        }

        protected virtual void AddItem(string key, object value)
        {
            lock (Semaphore)
            {
                Cache.Add(key, value, _defaultExpiry);
            }
        }

        protected virtual void UpdateItem(string key, object value)
        {
            lock (Semaphore)
            {
                Cache.Set(key, value, _defaultExpiry);
            }
        }

        protected virtual void RemoveItem(string key)
        {
            lock (Semaphore)
            {
                if (Cache.Contains(key))
                    Cache.Remove(key);
            }
        }

        protected virtual object GetItem(string key, bool remove)
        {
            lock (Semaphore)
            {
                var value = Cache[key];

                if (null == value)
                {
                    Logger.Info($"No item in the cache with the key \"{key}\"");
                    return null;
                }
                if (remove)
                    RemoveItem(key); // Not that you need another lock, just a check to see if it exists first.
                else
                {
                    WriteToLog($"The CachingProvider does not contain the cache key: {key}");
                }
                return value;
            }
        }

        #region Error Logs

        protected void WriteToLog(string text)
        {
            if (cs.AppIsInDebugMode)
                Logger.Info(text);
        }

        #endregion
    }
}