using System;
using System.Collections.Specialized;
using System.Configuration;

namespace BridgeportClaims.Business.Config
{
    public class ConfigService : IConfigService
    {
        public NameValueCollection GetAllConfigItems()
        {
            var collection = new NameValueCollection();
            foreach (var configItemKey in ConfigurationManager.AppSettings.AllKeys)
            {
                var value = ConfigurationManager.AppSettings[configItemKey];
                collection.Add(configItemKey, value);
            }
            return collection;
        }

        public string GetConfigItem(string key) => ConfigurationManager.AppSettings[key];

        public bool ApplicationIsInDebugMode
        {
            get
            {
                bool b;
                var debugMode = GetAllConfigItems()["ApplicationIsInDebugMode"];
                return Boolean.TryParse(debugMode, out b) && b;
            }
        }
    }
}
