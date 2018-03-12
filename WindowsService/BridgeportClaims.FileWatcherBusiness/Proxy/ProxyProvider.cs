using NLog;
using System;
using System.Data;
using System.Reflection;
using BridgeportClaims.FileWatcherBusiness.IO;
using BridgeportClaims.FileWatcherBusiness.DAL;
using BridgeportClaims.FileWatcherBusiness.Enums;
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

        private void MergeDocuments(DataTable dt)
        {
            try
            {
                _imageDataProvider.MergeDocuments(dt);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public void InitializeFirstImageFileTraversalIfNecessary(FileType fileType)
        {
            try
            {
                var method = MethodBase.GetCurrentMethod().Name;
                var now = DateTime.Now.ToString(LoggingService.TimeFormat);
                var doInitialFileTraversal = cs.GetAppSetting(c.PerformInitialDirectoryTraversalKey);
                var initial = bool.TryParse(doInitialFileTraversal, out var b) && b;
                var rootDomain = cs.GetAppSetting(fileType == FileType.Images ? c.ImagesRootDomainNameKey :
                    fileType == FileType.Invoices ? c.InvoicesRootDomainNameKey :
                    throw new Exception($"Error, the {nameof(fileType)} arguement passed in is not a valid type."));
                if (rootDomain.IsNullOrWhiteSpace())
                {
                    throw new Exception(
                        $"Error, could not get the root domain from the config file within {method} method on {now}.");
                }
                var fileLocation = cs.GetAppSetting(fileType == FileType.Images ? c.ImagesFileLocationKey :
                    fileType == FileType.Invoices ? c.InvoicesFileLocationKey : throw new Exception($"Error, the {nameof(fileType)} arguement passed in is not a valid type."));
                if (fileLocation.IsNullOrWhiteSpace())
                {
                    throw new Exception(
                        $"Error, could not get the file location from the config file within the {method} method on {now}.");
                }
                if (!initial)
                {
                    return;
                }
                var dt = IoHelper.TraverseDirectories(fileLocation, rootDomain, fileType)?.ToDataTable();
                if (null != dt)
                {
                    MergeDocuments(dt);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
    }
}