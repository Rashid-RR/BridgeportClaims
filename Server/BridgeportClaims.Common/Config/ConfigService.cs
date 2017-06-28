using System;
using System.Collections.Specialized;
using cm = System.Configuration.ConfigurationManager;
using c = BridgeportClaims.Common.StringConstants.Constants;

namespace BridgeportClaims.Common.Config
{
    public static class ConfigService
    {
        public static NameValueCollection GetAllAppSettings()
        {
            var collection = new NameValueCollection();
            foreach (var configItemKey in cm.AppSettings.AllKeys)
                collection.Add(configItemKey, cm.AppSettings[configItemKey]);
            return collection;
        }

        public static string GetAppSetting(string key) => cm.AppSettings[key];

        public static string GetDbConnStr() 
            => cm.ConnectionStrings[c.DbConnStrName].ConnectionString;

        public static bool AppIsInDebugMode
            => Convert.ToBoolean(GetAppSetting(c.AppIsInDebugMode));
    }
}