using System.Data;
using BridgeportClaims.FileWatcherBusiness.DAL;

namespace BridgeportClaims.FileWatcherBusiness.Proxy
{
    public class ProxyProvider
    {
        private readonly ImageDataProvider _imageDataProvider;

        public ProxyProvider()
        {
            _imageDataProvider = new ImageDataProvider();
        }

        public void MergeDocuments(DataTable dt)
        {
            _imageDataProvider.MergeDocuments(dt);
        }
    }
}