using System.ComponentModel;
using System.ServiceProcess;

namespace BridgeportClaimsService.FileWatcherService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void BridgeportClaimsWindowsServiceInstaller_AfterInstall(object sender, System.Configuration.Install.InstallEventArgs e)
        {
            new ServiceController(BridgeportClaimsWindowsServiceInstaller.ServiceName).Start();
        }

        private void BridgeportClaimsWindowsServiceProcessInstaller_AfterInstall(object sender, System.Configuration.Install.InstallEventArgs e)
        {

        }
    }
}
