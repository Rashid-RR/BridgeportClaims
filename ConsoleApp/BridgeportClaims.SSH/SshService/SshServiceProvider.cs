using System.Collections.Generic;
using System.Linq;
using BridgeportClaims.SSH.Disposable;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using BridgeportClaims.SSH.Models;

namespace BridgeportClaims.SSH.SshService
{
    public static class SshServiceProvider
    {
        private static IList<SftpFile> ListSshFiles(ConnectionInfo connectionInfo, string remoteSftpFilePath)
        {
            return DisposableService.Using(() => new SftpClient(connectionInfo), client =>
            {
                if (null == client)
                    return null;
                client.Connect();
                var ftpDirectoryListing = client.ListDirectory(remoteSftpFilePath)?.ToList();
                // TODO: var remoteDirectory = // TODO:  $"{path}/{sftpFileName}";
                // TODO: var downloadedFile = File.OpenRead(remoteDirectory + sftpFileName.FileName);
                // TODO: client.DownloadFile(remoteDirectory, downloadedFile);

                return ftpDirectoryListing;
            });
        }

        public static void ProcessSftpOperation(SftpConnectionModel model, string remoteSftpFilePath, string localSftpDownloadDirectoryFullPath)
        {
            var sftpFiles = ListSshFiles(GetConnectionInfo(model), localSftpDownloadDirectoryFullPath);
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