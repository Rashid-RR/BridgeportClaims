using NLog;
using System;
using System.Reflection;
using BridgeportClaims.SSH.Models;
using BridgeportClaims.SSH.SshService;
using LakerFileImporter.Enums;
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

        internal void ProcessLakerSftpOperation()
        {
            var methodName = MethodBase.GetCurrentMethod().Name;
            var className = GetType().Name;
            var now = DateTime.Now.ToString("G");
            if (cs.AppIsInDebugMode)
            {
                Logger.Info($"Starting into the {methodName} method, within the {className} class, on {now}.");
            }

            var sftpRemoteSitePath = cs.GetAppSetting(c.LakerSftpRemoteSitePathKey);
            // This includes the month and year folder.
            var localDirectoryDownloadFullPath = IoHelper.GetFullLocalFilePathPlusMonthYearFolderByDate(DateTime.Now, FileSource.Laker);
            // Process SFTP Operation
            var fileProcessorTopNumber =
                int.TryParse(cs.GetAppSetting(c.FileProcessorTopNumberKey), out var ti) ? ti : 10;
            SshServiceProvider.ProcessLakerSftpOperation(LakerSftpConnectionModel, sftpRemoteSitePath,
                localDirectoryDownloadFullPath, fileProcessorTopNumber);
        }

        internal void ProcessEnvisionSftpOperation()
        {
            var methodName = MethodBase.GetCurrentMethod().Name;
            var className = GetType().Name;
            var now = DateTime.Now.ToString("G");
            if (cs.AppIsInDebugMode)
            {
                Logger.Info($"Starting into the {methodName} method, within the {className} class, on {now}.");
            }

            var sftpRemoteSitePath = cs.GetAppSetting(c.LakerSftpRemoteSitePathKey);
            // This includes the month and year folder.
            var localDirectoryDownloadFullPath = IoHelper.GetFullLocalFilePathPlusMonthYearFolderByDate(DateTime.Now, FileSource.Envision);
            // Process SFTP Operation
            var fileProcessorTopNumber =
                int.TryParse(cs.GetAppSetting(c.FileProcessorTopNumberKey), out var ti) ? ti : 10;
            SshServiceProvider.ProcessEnvisionSftpOperation(EnvisionSftpConnectionModel, sftpRemoteSitePath,
                localDirectoryDownloadFullPath, fileProcessorTopNumber);
        }

        private static SftpConnectionModel LakerSftpConnectionModel
        {
            get
            {
                var host = cs.GetAppSetting(c.LakerSftpHostKey);
                var userName = cs.GetAppSetting(c.LakerSftpUserNameKey);
                var password = new CompiledSecurityProvider().RawLakerSftpPassword;
                var remoteSitePath = cs.GetAppSetting(c.LakerSftpRemoteSitePathKey);
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

        private static SftpConnectionModel EnvisionSftpConnectionModel
        {
            get
            {
                var host = cs.GetAppSetting(c.EnvisionSftpHostKey);
                var userName = cs.GetAppSetting(c.EnvisionSftpUserNameKey);
                var password = new CompiledSecurityProvider().RawEnvisionSftpPassword;
                var remoteSitePath = cs.GetAppSetting(c.EnvisionSftpRemoteSitePathKey);
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