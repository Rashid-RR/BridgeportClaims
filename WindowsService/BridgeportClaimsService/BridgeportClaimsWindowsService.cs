using System.ServiceProcess;

namespace BridgeportClaimsService
{
    public partial class BridgeportClaimsWindowsService : ServiceBase
    {
        public BridgeportClaimsWindowsService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }
    }
}
