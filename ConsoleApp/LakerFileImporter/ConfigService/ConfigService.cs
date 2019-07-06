using System;
using System.Collections.Specialized;
using LakerFileImporter.Security;
using cm = System.Configuration.ConfigurationManager;
using c = LakerFileImporter.StringConstants.Constants;

namespace LakerFileImporter.ConfigService
{
    internal static class ConfigService
    {
        internal static NameValueCollection GetAllAppSettings()
        {
            var collection = new NameValueCollection();
            foreach (var configItemKey in cm.AppSettings.AllKeys)
                collection.Add(configItemKey, cm.AppSettings[configItemKey]);
            return collection;
        }

        internal static string GetAppSetting(string key) => cm.AppSettings[key];

        internal static string GetDbConnStr() 
            => new CompiledSecurityProvider().RawConnectionString;

        internal static bool AppIsInDebugMode
            => Convert.ToBoolean(GetAppSetting(c.AppIsInDebugMode)?.ToLower());

        internal static string GetAppSetting(object envisionFileProcessingCount)
        {
            throw new NotImplementedException();
        }
    }
}