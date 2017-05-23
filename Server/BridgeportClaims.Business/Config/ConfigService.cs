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

        public bool ApplicationIsInDebugMode
        {
            get
            {
                var debugMode = GetAllConfigItems()["ApplicationIsInDebugMode"];
                // Parsed accurately, and is true.
                return bool.TryParse(debugMode, out bool b) && b;
            }
        }
    }
}
