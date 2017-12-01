using System.IO;
using System.ServiceProcess;
using BridgeportClaims.FileWatcherBusiness.Providers;

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
            _fileWatcherProvider = new FileWatcherProvider();
        }

        protected override void OnStop()
        {
        }
    }
}
