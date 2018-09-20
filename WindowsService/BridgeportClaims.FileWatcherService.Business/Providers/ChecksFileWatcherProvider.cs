using BridgeportClaims.Business.Enums;

namespace BridgeportClaims.Business.Providers
{
    public class ChecksFileWatcherProvider : FileWatcherProvider
    {
        public ChecksFileWatcherProvider() : base(FileType.Checks) { }
    }
}
