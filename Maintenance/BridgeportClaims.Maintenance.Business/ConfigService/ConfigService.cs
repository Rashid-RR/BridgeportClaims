using System;
using System.Collections.Specialized;
using c = BridgeportClaims.Maintenance.Business.StringConstants.Constants;
using cm = System.Configuration.ConfigurationManager;

namespace BridgeportClaims.Maintenance.Business.ConfigService
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

        internal static string GetDbConnStr() 
            => cm.ConnectionStrings[c.DbConnStrName].ConnectionString;

        public static bool AppIsInDebugMode
            => Convert.ToBoolean(GetAppSetting(c.AppIsInDebugMode)?.ToLower());
    }
}