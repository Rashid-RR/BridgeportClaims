using System.IO;
using System.ServiceProcess;

namespace BridgeportClaimsService.FileWatcherService
{
    public partial class BridgeportClaimsWindowsService : ServiceBase
    {
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
            File.Create(@"C:\Users\Public\Documents\OnStart.txt");
        }

        protected override void OnStop()
        {
            File.Create(@"C:\Users\Public\Documents\OnStop.txt");
        }
    }
}
