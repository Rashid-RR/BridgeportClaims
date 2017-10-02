using System;
using System.Collections.Specialized;
using cm = System.Configuration.ConfigurationManager;
using c = LakerFileImporter.StringConstants.Constants;

namespace LakerFileImporter.ConfigService
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