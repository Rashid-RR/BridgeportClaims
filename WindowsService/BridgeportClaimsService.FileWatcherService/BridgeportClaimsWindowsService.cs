using System.IO;
using System.ServiceProcess;
using BridgeportClaims.FileWatcherBusiness.Providers;

namespace BridgeportClaimsService.FileWatcherService
{
    public partial class BridgeportClaimsWindowsService : ServiceBase
    {
        private readonly FileWatcherProvider _fileWatcherProvider;
        public BridgeportClaimsWindowsService()
        {
            InitializeComponent();
            _fileWatcherProvider = new FileWatcherProvider();
        }

        internal void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            File.Create(@"C:\Users\Public\Documents\OnStart.txt");
        }

        protected override void OnStop()
        {
            File.Create(@"C:\Users\Public\Documents\OnStop.txt");
        }
    }
}
