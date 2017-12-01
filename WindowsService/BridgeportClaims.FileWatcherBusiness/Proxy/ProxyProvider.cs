using System;
using System.Data;
using BridgeportClaims.FileWatcherBusiness.DAL;
using BridgeportClaims.FileWatcherBusiness.Extensions;
using BridgeportClaims.FileWatcherBusiness.IO;
using BridgeportClaims.FileWatcherBusiness.StringConstants;
using cs = BridgeportClaims.FileWatcherBusiness.ConfigService.ConfigService;

namespace BridgeportClaims.FileWatcherBusiness.Proxy
{
    public class ProxyProvider
    {
        private readonly ImageDataProvider _imageDataProvider;
        private const string False = "false";

        public ProxyProvider()
        {
            _imageDataProvider = new ImageDataProvider();
        }

        public void MergeDocuments(DataTable dt)
        {
            _imageDataProvider.MergeDocuments(dt);
        }

        public void InitializeFirstFileTraversalIfNecessary()
        {
            var doInitialFileTraversal = cs.GetAppSetting(Constants.PerformInitialDirectoryTraversalKey);
            var initial = Convert.ToBoolean(doInitialFileTraversal.IsNotNullOrWhiteSpace() ? doInitialFileTraversal.ToLower() : False);
            var rootDomain = cs.GetAppSetting(Constants.ImagesRootDomainNameKey);
            var fileLocation = cs.GetAppSetting(Constants.FileLocationKey);
            if (!initial) return;
            var dt = IoHelper.TraverseDirectories(fileLocation, rootDomain)?.ToDataTable();
            if (null != dt)
                MergeDocuments(dt);
        }
    }
}