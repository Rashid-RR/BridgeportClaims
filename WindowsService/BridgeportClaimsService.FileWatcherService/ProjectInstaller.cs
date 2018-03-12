using System.ComponentModel;
using System.ServiceProcess;

namespace BridgeportClaimsService.FileWatcherService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            /*var process = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem,
                Username = null,
                Password = null
            };
            var serviceAdmin = new ServiceInstaller
            {
                StartType = ServiceStartMode.Automatic,
                ServiceName = "filewatcher",
                DisplayName = "File Watcher"
            };
            Installers.Add(process);
            Installers.Add(serviceAdmin);*/
            InitializeComponent();
        }

        private void BridgeportClaimsWindowsServiceInstaller_AfterInstall(object sender,
            System.Configuration.Install.InstallEventArgs e)
        {
            //using (var serviceController = new ServiceController())
            //{
            //    serviceController.Start();
            //}
        }
    }
}