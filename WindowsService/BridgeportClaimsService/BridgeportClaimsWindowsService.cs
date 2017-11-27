using System;
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
            File.Create(AppDomain.CurrentDomain.BaseDirectory + "OnStart.txt");
        }

        protected override void OnStop()
        {
            File.Create(AppDomain.CurrentDomain.BaseDirectory + "OnStop.txt");
        }
    }
}
