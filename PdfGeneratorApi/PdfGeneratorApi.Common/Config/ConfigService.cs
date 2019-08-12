using System;
using System.Collections.Specialized;
using cm = System.Configuration.ConfigurationManager;
using s = PdfGeneratorApi.Common.Constants.StringConstants;

namespace PdfGeneratorApi.Common.Config
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
        public static string GetAppSetting(string key) => cm.AppSettings[key];
        public static string GetDbConnStr()
            => cm.ConnectionStrings[s.DbConnStrName].ConnectionString;
        public static string PdfDropPath => GetAppSetting(s.PdfDropPath);
        public static bool AppIsInDebugMode
            => Convert.ToBoolean(GetAppSetting(s.AppIsInDebugMode));
    }
}