using System.Collections.Specialized;

namespace BridgeportClaims.Business.Config
{
    public interface IConfigService
    {
        NameValueCollection GetAllConfigItems();
        string GetConfigItem(string key);
        bool ApplicationIsInDebugMode { get; }
    }
}