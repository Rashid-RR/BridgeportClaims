using System.Collections.Generic;
using System.IO;
using System.Linq;
using BridgeportClaims.SSH.Disposable;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using BridgeportClaims.SSH.Models;

namespace BridgeportClaims.SSH.SshService
{
    public static class SshServiceProvider
    {
        private static readonly string ComparisonString = "ENVexport_BPC_".ToLowerInvariant();
        private static readonly int Count = "ENVexport_BPC_20190622080001.csv".Length;

        private static IList<SftpFile> ListLastTenEnvisionSshFiles(ConnectionInfo connectionInfo,
            string remoteSftpFilePath, int fileProcessorTopNumber)
            => DisposableService.Using(() => new SftpClient(connectionInfo), client =>
            {
                if (null == client)
                {
                    return null;
                }

                client.Connect();
                var ftpDirectoryListing = client.ListDirectory(remoteSftpFilePath)?.ToList();
                return ftpDirectoryListing?.Where(f => !string.IsNullOrWhiteSpace(f.Name) &&
                                                       f.Name.ToLower().StartsWith(ComparisonString)
                                                       && f.Name.Length == Count)
                    .OrderByDescending(f => f.Name).Take(fileProcessorTopNumber).ToList();
            });

        private static IList<SftpFile> ListLastTenLakerSshFiles(ConnectionInfo connectionInfo,
            string remoteSftpFilePath, int fileProcessorTopNumber)
        {
            return DisposableService.Using(() => new SftpClient(connectionInfo), client =>
            {
                if (null == client)
                {
                    return null;
                }

                client.Connect();

                var ftpDirectoryListing = client.ListDirectory(remoteSftpFilePath)?.ToList();
                return ftpDirectoryListing?.Where(f => !string.IsNullOrWhiteSpace(f.Name) &&
                                                       f.Name.ToLower().StartsWith("billing_claim_file_"))
                    .OrderByDescending(f => f.Name).Take(fileProcessorTopNumber).ToList();
            });
        }

        public static void ProcessEnvisionSftpOperation(SftpConnectionModel model, string remoteSftpFilePath,
            string localSftpDownloadDirectoryFullPath, int fileProcessorTopNumber)
        {
            var connectionInfo = GetEnvisionConnectionInfo(model);
            var lastTenEnvisionSftpFiles = ListLastTenEnvisionSshFiles(GetEnvisionConnectionInfo(model),
                remoteSftpFilePath, fileProcessorTopNumber);
            DisposableService.Using(() => new SftpClient(connectionInfo), client =>
            {
                client.Connect();
                foreach (var sftpFile in lastTenEnvisionSftpFiles)
                {
                    var fullLocalFileName = Path.Combine(localSftpDownloadDirectoryFullPath, sftpFile.Name);
                    if (File.Exists(fullLocalFileName))
                    {
                        continue;
                    }

                    DisposableService.Using<Stream>(() => File.Create(fullLocalFileName),
                        fileStream => { client.DownloadFile(remoteSftpFilePath + "/" + sftpFile.Name, fileStream); });
                }
            });
        }

        public static void ProcessLakerSftpOperation(SftpConnectionModel model, string remoteSftpFilePath,
            string localSftpDownloadDirectoryFullPath, int fileProcessorTopNumber)
        {
            var connectionInfo = GetLakerConnectionInfo(model);
            var lastTenLakerSftpFiles = ListLastTenLakerSshFiles(GetLakerConnectionInfo(model), remoteSftpFilePath,
                fileProcessorTopNumber);
            DisposableService.Using(() => new SftpClient(connectionInfo), client =>
            {
                client.Connect();
                foreach (var sftpFile in lastTenLakerSftpFiles)
                {
                    var fullLocalFileName = Path.Combine(localSftpDownloadDirectoryFullPath, sftpFile.Name);
                    if (File.Exists(fullLocalFileName))
                    {
                        continue;
                    }

                    DisposableService.Using<Stream>(() => File.Create(fullLocalFileName),
                        fileStream => { client.DownloadFile(remoteSftpFilePath + "/" + sftpFile.Name, fileStream); });
                }
            });
        }

        private static ConnectionInfo GetLakerConnectionInfo(SftpConnectionModel model)
        {
            var connectionInfo = null != model.Port
                ? new ConnectionInfo(model.Host, model.Port.Value, model.UserName,
                    new PasswordAuthenticationMethod(model.UserName, model.Password))
                : new ConnectionInfo(model.Host, model.UserName,
                    new PasswordAuthenticationMethod(model.UserName, model.Password));
            return connectionInfo;
        }

        private static ConnectionInfo GetEnvisionConnectionInfo(SftpConnectionModel model)
        {
            return null != model.Port
                ? new ConnectionInfo(model.Host, model.Port.Value, model.UserName,
                    new PasswordAuthenticationMethod(model.UserName, model.Password))
                : new ConnectionInfo(model.Host, model.UserName,
                    new PasswordAuthenticationMethod(model.UserName, model.Password));
        }
    }
}