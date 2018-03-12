using BridgeportClaims.Business.Enums;

namespace BridgeportClaims.Business.Providers
{
    public class ImageFileWatcherProvider : FileWatcherProvider
    {
        public ImageFileWatcherProvider() : base(FileType.Images) { }
    }
}