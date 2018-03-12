using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.ServiceProcess;
using BridgeportClaims.Business.Enums;
using BridgeportClaims.Business.Logging;
using BridgeportClaims.Business.Providers;
using BridgeportClaims.Business.Proxy;

namespace BridgeportClaims.FileWatcherService
{
    [SuppressMessage("ReSharper", "NotAccessedField.Local")]
    public partial class ServiceInstaller : ServiceBase
    {
        private ImageFileWatcherProvider _imageFileWatcherProvider;
        private InvoiceFileWatcherProvider _invoiceFileWatcherProvider;
        private static readonly LoggingService LoggingService = LoggingService.Instance;

        public ServiceInstaller()
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
                Debugger.Break();
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
