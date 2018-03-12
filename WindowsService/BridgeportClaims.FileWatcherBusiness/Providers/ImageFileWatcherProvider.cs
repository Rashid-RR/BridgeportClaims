using BridgeportClaims.FileWatcherBusiness.Enums;

namespace BridgeportClaims.FileWatcherBusiness.Providers
{
    public class ImageFileWatcherProvider : FileWatcherProvider
    {
        public ImageFileWatcherProvider() : base(FileType.Images) { }
    }
}