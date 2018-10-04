using System;
using System.Collections.Specialized;
using BridgeportClaims.Common.Constants;
using cm = System.Configuration.ConfigurationManager;
using s = BridgeportClaims.Common.Constants.StringConstants;

namespace BridgeportClaims.Common.Config
{
    public static class ConfigService
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

        public static string GetAppSetting(string key) => cm.AppSettings[key];

        public static string GetDbConnStr() 
            => cm.ConnectionStrings[StringConstants.DbConnStrName].ConnectionString;

        public static string GetSecureDbConnStr()
            => cm.ConnectionStrings[StringConstants.SecureDbConnStrName].ConnectionString;

        public static bool AppIsInDebugMode
            => Convert.ToBoolean(GetAppSetting(StringConstants.AppIsInDebugMode));

        public static string GetRedisCacheConnStr()
            => GetAppSetting(s.RedisCacheConnection);
    }
}