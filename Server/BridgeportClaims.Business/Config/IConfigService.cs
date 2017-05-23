using System.Collections.Specialized;

namespace BridgeportClaims.Business.Config
{
    public interface IConfigService
    {
        NameValueCollection GetAllConfigItems();
        bool ApplicationIsInDebugMode { get; }
    }
}