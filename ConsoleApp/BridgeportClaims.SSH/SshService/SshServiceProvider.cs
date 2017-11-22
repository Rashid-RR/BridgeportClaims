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
        private static IList<SftpFile> ListLastTenSshFiles(ConnectionInfo connectionInfo, string remoteSftpFilePath)
        {
            return DisposableService.Using(() => new SftpClient(connectionInfo), client =>
            {
                if (null == client)
                    return null;
                client.Connect();
                var ftpDirectoryListing = client.ListDirectory(remoteSftpFilePath)?.ToList();
                return ftpDirectoryListing?.Where(f => !string.IsNullOrWhiteSpace(f.Name) &&
                                          f.Name.ToLower().StartsWith("billing_claim_file_"))
                    .OrderByDescending(f => f.Name).Take(10).ToList();
            });
        }

        public static void ProcessSftpOperation(SftpConnectionModel model, string remoteSftpFilePath, string localSftpDownloadDirectoryFullPath)
        {
            var connectionInfo = GetConnectionInfo(model);
            var lastTenSftpFiles = ListLastTenSshFiles(GetConnectionInfo(model), remoteSftpFilePath);
            DisposableService.Using(() => new SftpClient(connectionInfo), client =>
            {
                client.Connect();
                foreach (var sftpFile in lastTenSftpFiles)
                {
                    var fullLocalFileName = Path.Combine(localSftpDownloadDirectoryFullPath, sftpFile.Name);
                    if (File.Exists(fullLocalFileName))
                        continue;

                    DisposableService.Using<Stream>(() => File.Create(fullLocalFileName), fileStream =>
                    {
                        client.DownloadFile(remoteSftpFilePath + "/" + sftpFile.Name, fileStream);
                    });
                }
            });
        }

        public static ConnectionInfo GetConnectionInfo(SftpConnectionModel model)
        {
            var connectionInfo = null != model.Port
                ? new ConnectionInfo(model.Host, model.Port.Value, model.UserName, new PasswordAuthenticationMethod(model.UserName, model.Password))
                : new ConnectionInfo(model.Host, model.UserName, new PasswordAuthenticationMethod(model.UserName, model.Password));
            return connectionInfo;
        }
    }
}