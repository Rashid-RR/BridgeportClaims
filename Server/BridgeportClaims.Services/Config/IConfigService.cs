using System.Collections.Specialized;

namespace BridgeportClaims.Services.Config
{
    public interface IConfigService
    {
        NameValueCollection GetAllConfigItems();
        string GetConfigItem(string key);
        bool ApplicationIsInDebugMode { get; }
    }
}