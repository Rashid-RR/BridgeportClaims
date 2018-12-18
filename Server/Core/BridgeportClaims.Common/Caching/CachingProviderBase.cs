using NLog;
using System;
using System.Runtime.Caching;
using BridgeportClaims.Common.Constants;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Common.Caching
{
    public abstract class CachingProviderBase
    {
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);
        protected Lazy<MemoryCache> Cache = new Lazy<MemoryCache>(() => new MemoryCache(StringConstants.CachingProvider));
        private static readonly object Semaphore  = new object();
        private readonly DateTimeOffset _defaultExpiry;

        protected CachingProviderBase()
        {
            _defaultExpiry = DateTimeOffset.UtcNow.AddDays(14);
        }

        protected virtual void UpdateItem(string key, object value)
        {
            lock (Semaphore)
            {
                Cache.Value.Set(key, value, _defaultExpiry);
            }
        }

        protected virtual void RemoveItem(string key)
        {
            lock (Semaphore)
            {
                if (Cache.Value.Contains(key))
                {
                    Cache.Value.Remove(key);
                }
            }
        }

        #region Error Logs

        protected void WriteToLog(string text)
        {
            if (cs.AppIsInDebugMode)
            {
                Logger.Value.Info(text);
            }
        }

        #endregion
    }
}