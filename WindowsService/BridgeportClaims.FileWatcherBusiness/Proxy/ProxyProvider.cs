using NLog;
using System;
using System.Data;
using System.Reflection;
using BridgeportClaims.FileWatcherBusiness.IO;
using BridgeportClaims.FileWatcherBusiness.DAL;
using BridgeportClaims.FileWatcherBusiness.Logging;
using BridgeportClaims.FileWatcherBusiness.Extensions;
using cs = BridgeportClaims.FileWatcherBusiness.ConfigService.ConfigService;
using c = BridgeportClaims.FileWatcherBusiness.StringConstants.Constants;

namespace BridgeportClaims.FileWatcherBusiness.Proxy
{
    public class ProxyProvider
    {
        private readonly ImageDataProvider _imageDataProvider;
        private readonly ILogger _logger = LoggingService.Instance.Logger;

        public ProxyProvider()
        {
            _imageDataProvider = new ImageDataProvider();
        }

        public void MergeDocuments(DataTable dt)
        {
            try
            {
                _imageDataProvider.MergeDocuments(dt);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        public void InitializeFirstFileTraversalIfNecessary()
        {
            try
            {
                var method = MethodBase.GetCurrentMethod().Name;
                var now = DateTime.Now.ToString(LoggingService.TimeFormat);
                var doInitialFileTraversal = cs.GetAppSetting(c.PerformInitialDirectoryTraversalKey);
                var initial = bool.TryParse(doInitialFileTraversal, out bool b) && b;
                var rootDomain = cs.GetAppSetting(c.ImagesRootDomainNameKey);
                if (rootDomain.IsNullOrWhiteSpace())
                    throw new Exception($"Error, could not get the root domain from the config file within {method} method on {now}.");
                var fileLocation = cs.GetAppSetting(c.FileLocationKey);
                if (fileLocation.IsNullOrWhiteSpace())
                    throw new Exception($"Error, could not get the file location from the config file within the {method} method on {now}.");
                if (!initial) return;
                var dt = IoHelper.TraverseDirectories(fileLocation, rootDomain)?.ToDataTable();
                if (null != dt)
                    MergeDocuments(dt);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }
    }
}