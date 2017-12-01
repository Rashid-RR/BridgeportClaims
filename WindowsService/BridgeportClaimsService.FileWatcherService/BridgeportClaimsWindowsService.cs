using System.ServiceProcess;
using BridgeportClaims.FileWatcherBusiness.Providers;
using BridgeportClaims.FileWatcherBusiness.Proxy;

namespace BridgeportClaimsService.FileWatcherService
{
    public partial class BridgeportClaimsWindowsService : ServiceBase
    {
        private FileWatcherProvider _fileWatcherProvider;
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
            var proxyProvider = new ProxyProvider();
            proxyProvider.InitializeFirstFileTraversalIfNecessary();
            _fileWatcherProvider = new FileWatcherProvider();
        }

        protected override void OnStop()
        {
        }
    }
}
