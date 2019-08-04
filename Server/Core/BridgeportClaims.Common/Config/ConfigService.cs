using System;
using System.Collections.Specialized;
using cm = System.Configuration.ConfigurationManager;
using s = BridgeportClaims.Common.Constants.StringConstants;

namespace BridgeportClaims.Common.Config
{
    public class ConfigService
    {
        public static NameValueCollection GetAllAppSettings()
        {
            var collection = new NameValueCollection();
            foreach (var configItemKey in cm.AppSettings.AllKeys)
            {
                collection.Add(configItemKey, cm.AppSettings[configItemKey]);
            }
            return collection;
        }
        public static bool IsProduction = Convert.ToBoolean(GetAppSetting(s.IsProductionKey));
        public static bool UseRedis => Convert.ToBoolean(GetAppSetting(s.UseRedisKey));
        public static string CacheConnection => GetAppSetting(s.RedisCacheConnection);
        public static string GetAppSetting(string key) => cm.AppSettings[key];
        public static string GetDbConnStr() 
            => cm.ConnectionStrings[s.DbConnStrName].ConnectionString;
        public static string GetSecureDbConnStr()
            => cm.ConnectionStrings[s.SecureDbConnStrName].ConnectionString;
        public static bool AppIsInDebugMode
            => Convert.ToBoolean(GetAppSetting(s.AppIsInDebugMode));
    }
}