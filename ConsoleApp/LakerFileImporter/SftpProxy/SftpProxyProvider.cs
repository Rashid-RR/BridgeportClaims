using NLog;
using System;
using System.Reflection;
using BridgeportClaims.SSH.Models;
using BridgeportClaims.SSH.SshService;
using LakerFileImporter.IO;
using LakerFileImporter.Logging;
using LakerFileImporter.Security;
using cs = LakerFileImporter.ConfigService.ConfigService;
using c = LakerFileImporter.StringConstants.Constants;

namespace LakerFileImporter.SftpProxy
{
    internal sealed class SftpProxyProvider
    {
        private static readonly Logger Logger = LoggingService.Instance.Logger;

            internal void ProcessSftpOperation()
        {
            var methodName = MethodBase.GetCurrentMethod().Name;
            var className = GetType().Name;
            var now = DateTime.Now.ToString("G");
            if (cs.AppIsInDebugMode)
                Logger.Info($"Starting into the {methodName} method, within the {className} class, on {now}.");

            var sftpRemoteSitePath = cs.GetAppSetting(c.SftpRemoteSitePathKey);
            // This includes the month and year folder.
            var localDirectoryDownloadFullPath = new IoHelper().LocalFullFilePathWithMonthYearFolder;
            // Process SFTP Operation
            SshServiceProvider.ProcessSftpOperation(SftpConnectionModel, sftpRemoteSitePath, localDirectoryDownloadFullPath);
        }

        private static SftpConnectionModel SftpConnectionModel
        {
            get
            {
                var host = cs.GetAppSetting(c.SftpHostKey);
                var userName = cs.GetAppSetting(c.SftpUserNameKey);
                var password = new CompiledSecurityProvider().RawLakerSftpPassword;
                var remoteSitePath = cs.GetAppSetting(c.SftpRemoteSitePathKey);
                var port = cs.GetAppSetting(c.SftpPortKey);
                var portInt = !string.IsNullOrWhiteSpace(port) ? Convert.ToInt32(port) : (int?) null;
                return new SftpConnectionModel
                {
                    Host = host,
                    UserName = userName,
                    Password = password,
                    Port = portInt,
                    RemoteSitePath = remoteSitePath
                };
            }
        }
    }
}