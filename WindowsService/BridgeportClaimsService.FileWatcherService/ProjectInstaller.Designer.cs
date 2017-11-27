namespace BridgeportClaimsService.FileWatcherService
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BridgeportClaimsWindowsServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.BridgeportClaimsWindowsServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // BridgeportClaimsWindowsServiceProcessInstaller
            // 
            this.BridgeportClaimsWindowsServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this.BridgeportClaimsWindowsServiceProcessInstaller.Password = null;
            this.BridgeportClaimsWindowsServiceProcessInstaller.Username = null;
            // 
            // BridgeportClaimsWindowsServiceInstaller
            // 
            this.BridgeportClaimsWindowsServiceInstaller.ServiceName = "Bridgeport Claims Windows Service";
            this.BridgeportClaimsWindowsServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.BridgeportClaimsWindowsServiceInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.BridgeportClaimsWindowsServiceInstaller_AfterInstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.BridgeportClaimsWindowsServiceProcessInstaller,
            this.BridgeportClaimsWindowsServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller BridgeportClaimsWindowsServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller BridgeportClaimsWindowsServiceInstaller;
    }
}