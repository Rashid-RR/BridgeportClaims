using System;
using System.Data;
using System.Reflection;
using BridgeportClaims.Business.DAL;
using BridgeportClaims.Business.Enums;
using BridgeportClaims.Business.Extensions;
using BridgeportClaims.Business.IO;
using BridgeportClaims.Business.Logging;
using NLog;
using cs = BridgeportClaims.Business.ConfigService.ConfigService;
using c = BridgeportClaims.Business.StringConstants.Constants;

namespace BridgeportClaims.Business.Proxy
{
    public class ProxyProvider
    {
        private readonly DocumentDataProvider _documentDataProvider;
        private readonly ILogger _logger = LoggingService.Instance.Logger;

        public ProxyProvider()
        {
            _documentDataProvider = new DocumentDataProvider();
        }

        private void MergeDocuments(DataTable dt, FileType fileType)
        {
            try
            {
                _documentDataProvider.MergeDocuments(dt, fileType);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
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
                    MergeDocuments(dt, fileType);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }
    }
}