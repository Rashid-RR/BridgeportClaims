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
        protected CachingProviderBase() { }

        protected MemoryCache Cache = new MemoryCache(c.CachingProvider);
        private static readonly object Semaphore  = new object();

        protected virtual void AddItem(string key, object value)
        {
            lock (Semaphore)
            {
                Cache.Add(key, value, DateTimeOffset.MaxValue);
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