using System;
using System.Diagnostics.CodeAnalysis;
using System.ServiceProcess;
using BridgeportClaims.FileWatcherBusiness.Enums;
using BridgeportClaims.FileWatcherBusiness.Logging;
using BridgeportClaims.FileWatcherBusiness.Providers;
using BridgeportClaims.FileWatcherBusiness.Proxy;

namespace BridgeportClaimsService.FileWatcherService
{
    [SuppressMessage("ReSharper", "NotAccessedField.Local")]
    public partial class BridgeportClaimsWindowsService : ServiceBase
    {
        private ImageFileWatcherProvider _imageFileWatcherProvider;
        private InvoiceFileWatcherProvider _invoiceFileWatcherProvider;
        private static readonly LoggingService LoggingService = LoggingService.Instance;

        public BridgeportClaimsWindowsService()
        {
            InitializeComponent();
        }

        internal void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                var proxyProvider = new ProxyProvider();
                proxyProvider.InitializeFirstImageFileTraversalIfNecessary(FileType.Images);
                proxyProvider.InitializeFirstImageFileTraversalIfNecessary(FileType.Invoices);
                _imageFileWatcherProvider = new ImageFileWatcherProvider();
                _invoiceFileWatcherProvider = new InvoiceFileWatcherProvider();
            }
            catch (Exception ex)
            {
                LoggingService.Logger.Error(ex);
            }
        }

        protected override void OnStop()
        {
        }
    }
}
