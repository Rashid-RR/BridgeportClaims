using System;
using System.Data;
using System.Reflection;
using BridgeportClaims.Business.DAL;
using BridgeportClaims.Business.Enums;
using BridgeportClaims.Business.Extensions;
using BridgeportClaims.Business.Helpers.IO;
using BridgeportClaims.Business.Logging;
using NLog;
using cs = BridgeportClaims.Business.ConfigService.ConfigService;
using c = BridgeportClaims.Business.StringConstants.Constants;

namespace BridgeportClaims.Business.Proxy
{
    public class ProxyProvider : IProxyProvider
    {
        private readonly Lazy<IDocumentDataProvider> _documentDataProvider;
        private readonly Lazy<IIoHelper> _ioHelper;
        private readonly Lazy<ILogger> _logger;

        public ProxyProvider(Lazy<ILogger> logger,
            Lazy<IDocumentDataProvider> documentDataProvider,
            Lazy<IIoHelper> ioHelper)
        {
            _logger = logger;
            _documentDataProvider = documentDataProvider;
            _ioHelper = ioHelper;
        }

        private void MergeDocuments(DataTable dt, FileType fileType)
        {
            try
            {
                _documentDataProvider.Value.MergeDocuments(dt, fileType);
            }
            catch (Exception ex)
            {
                _logger.Value.Error(ex);
                throw;
            }
        }

        public void InitializeFirstImageFileTraversalIfNecessary(FileType fileType)
        {
            try
            {
                var method = MethodBase.GetCurrentMethod().Name;
                var now = DateTime.Now.ToString(LoggingService.TimeFormat);
                if (cs.AppIsInDebugMode)
                {
                    _logger.Value.Info($"Starting the {method} method on {now}.");
                }
                var doInitialFileTraversal = cs.GetAppSetting(c.PerformInitialDirectoryTraversalKey);
                var initial = bool.TryParse(doInitialFileTraversal, out var b) && b;
                var rootDomain = cs.GetRootDomainByFileType(fileType);
                if (rootDomain.IsNullOrWhiteSpace())
                {
                    throw new Exception(
                        $"Error, could not get the root domain from the config file within {method} method on {now}.");
                }
                var fileLocation = cs.GetFileLocationByFileType(fileType);
                if (fileLocation.IsNullOrWhiteSpace())
                {
                    throw new Exception(
                        $"Error, could not get the file location from the config file within the {method} method on {now}.");
                }
                if (!initial)
                {
                    return;
                }
                var dt = _ioHelper.Value.TraverseDirectories(fileLocation, rootDomain, fileType)?.ToDataTable();
                if (null != dt)
                {
                    MergeDocuments(dt, fileType);
                }
            }
            catch (Exception ex)
            {
                _logger.Value.Error(ex);
                throw;
            }
        }
    }
}