using System;
using BridgeportClaims.Business.Enums;
using BridgeportClaims.Business.Providers;
using BridgeportClaims.Business.Proxy;
using NLog;

namespace BridgeportClaims.FileWatcherService.Host.Services
{
    public class FileWatcherWindowsService : IFileWatcherWindowsService
    {
        private readonly Lazy<IProxyProvider> _proxyProvider;
        private ImageFileWatcherProvider _imageFileWatcherProvider;
        private InvoiceFileWatcherProvider _invoiceFileWatcherProvider;
        private ChecksFileWatcherProvider _checksFileWatcherProvider;
        private readonly Lazy<ILogger> _logger;

        public FileWatcherWindowsService(Lazy<IProxyProvider> proxyProvider, Lazy<ILogger> logger)
        {
            _proxyProvider = proxyProvider;
            _logger = logger;
        }

        public void Start()
        {
            try
            {
                _proxyProvider.Value.InitializeFirstImageFileTraversalIfNecessary(FileType.Images);
                _proxyProvider.Value.InitializeFirstImageFileTraversalIfNecessary(FileType.Invoices);
                _proxyProvider.Value.InitializeFirstImageFileTraversalIfNecessary(FileType.Checks);
                _imageFileWatcherProvider = new ImageFileWatcherProvider();
                _invoiceFileWatcherProvider = new InvoiceFileWatcherProvider();
                _checksFileWatcherProvider = new ChecksFileWatcherProvider();
            }
            catch (Exception ex)
            {
                _logger.Value.Error(ex);
            }
            
        }

        public void Stop()
        {

        }
    }
}