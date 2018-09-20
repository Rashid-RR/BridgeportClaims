using BridgeportClaims.Business.Enums;

namespace BridgeportClaims.Business.Proxy
{
    public interface IProxyProvider
    {
        void InitializeFirstImageFileTraversalIfNecessary(FileType fileType);
    }
}