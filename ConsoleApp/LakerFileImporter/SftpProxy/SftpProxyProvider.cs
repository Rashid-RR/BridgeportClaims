﻿using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using BridgeportClaims.SSH.SshService;
using LakerFileImporter.Disposable;
using LakerFileImporter.Helpers;
using LakerFileImporter.Logging;
using NLog;
using Renci.SshNet;
using cs = LakerFileImporter.ConfigService.ConfigService;
using c = LakerFileImporter.StringConstants.Constants;

namespace LakerFileImporter.SftpProxy
{
    internal sealed class SftpProxyProvider
    {
        private static readonly Logger Logger = LoggingService.Instance.Logger;
        private const string NoSshFile = "Nothing found in the SSH Directory!";

        internal FileDateParsingHelper GetLatestFileFromFilePathOrSftp(FileDateParsingHelper newestFileInLocalDirectory)
        {
            var fileInDirectoryDoesntExist = null == newestFileInLocalDirectory;
            var methodName = MethodBase.GetCurrentMethod().Name;
            var className = GetType().Name;
            var now = DateTime.Now.ToString("G");
            if (cs.AppIsInDebugMode)
            {
                Logger.Info($"Starting into the {methodName} method, within the {className} class, on {now}.");
                if (fileInDirectoryDoesntExist)
                    Logger.Info(
                        "The was absolutely not file passed in as the newest file found in the local directory.");
                Logger.Info(
                    $"The newest file name that was found in the local directory was: {newestFileInLocalDirectory?.FileName}");
            }
            var sftpFilePath = cs.GetAppSetting(c.SftpRemoteSitePathKey);
            var directory = SshServiceProvider.TraverseSshDirectory(GetConnectionInfo(), sftpFilePath);
            if (null == directory && fileInDirectoryDoesntExist)
            {
                if (cs.AppIsInDebugMode)
                    Logger.Info(NoSshFile + ". And, no file exists in the local directory!");
                return null;
            }
            if (null == directory)
            {
                if (cs.AppIsInDebugMode)
                    Logger.Info(NoSshFile + ". So, we're forced to use the latest file in the local directory.");
                return newestFileInLocalDirectory;
            }
            // Ok, we've made it this far, there IS at least one file in the SSH directory.
            var directoryOfFiles = directory
                .Where(p => (p.Name.StartsWith("Billing_Claim_File_") ||
                             p.Name.StartsWith("/Billing_Claim_File_")) && p.Name.EndsWith(".csv"))
                .Select(s => new FileDateParsingHelper {FileName = s.Name, FullFileName = s.FullName}).ToList();
            var count = directoryOfFiles.Count;

            if (count > 0)
                if (cs.AppIsInDebugMode)
                    Logger.Info($"The SSH directory has a total of {count} files.");

            var newestSftpFile = directoryOfFiles.OrderByDescending(d =>
                d.LakerFileDate).Select(d => d).FirstOrDefault();
            if (null == newestSftpFile)
            {
                if (cs.AppIsInDebugMode)
                    Logger.Info("There was no newest file found within the SSH directory.");
                return !fileInDirectoryDoesntExist ? newestFileInLocalDirectory : null;
            }
            var localPath = cs.GetAppSetting(c.LakerFilePathKey);
            if (cs.AppIsInDebugMode)
                Logger.Info($"The most recent file found in the SSH directory is the file {newestSftpFile.FileName}");
            if (newestSftpFile.LakerFileDate == newestFileInLocalDirectory?.LakerFileDate)
                return ReturnFileFromSftp(newestSftpFile, localPath);
            if (newestSftpFile.LakerFileDate > newestFileInLocalDirectory?.LakerFileDate)
                return ReturnFileFromSftp(newestSftpFile, localPath);
            return newestFileInLocalDirectory?.LakerFileDate > newestSftpFile.LakerFileDate
                ? newestFileInLocalDirectory
                : null;
        }

        private static FileDateParsingHelper ReturnFileFromSftp(FileDateParsingHelper sftpFileName, string path)
        {
            if (cs.AppIsInDebugMode)
                Logger.Info(
                    "If our logic is correct, then the newest, SFTP file is newer than the latest file that we " +
                    "have on the file system, which means that it is time to download it and return it.");
            DisposableService.Using(() => new SftpClient(GetConnectionInfo()), client =>
            {
                client.Connect();
                var remoteDirectory = $"{path}/{sftpFileName}";
                var downloadedFile = File.OpenRead(remoteDirectory + sftpFileName.FileName);
                client.DownloadFile(remoteDirectory, downloadedFile);
            });
            return new FileDateParsingHelper();
        }

        private static ConnectionInfo GetConnectionInfo()
        {
            var host = cs.GetAppSetting(c.SftpHostKey);
            var userName = cs.GetAppSetting(c.SftpUserNameKey);
            var password = cs.GetAppSetting(c.SftpPasswordKey);
            var path = cs.GetAppSetting(c.SftpRemoteSitePathKey);
            var port = cs.GetAppSetting(c.SftpPortKey);
            var portInt = !string.IsNullOrWhiteSpace(port) ? Convert.ToInt32(port) : (int?) null;
            var connectionInfo = SshServiceProvider.GetConnectionInfo(host, userName, password, portInt, path);
            return connectionInfo;
        }
    }
}