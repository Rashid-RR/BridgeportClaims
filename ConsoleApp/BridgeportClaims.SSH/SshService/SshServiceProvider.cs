using System.Collections.Generic;
using System.Linq;
using BridgeportClaims.SSH.Disposable;
using Renci.SshNet;
using Renci.SshNet.Sftp;

namespace BridgeportClaims.SSH.SshService
{
    public static class SshServiceProvider
    {
        public static IList<SftpFile> TraverseSshDirectory(ConnectionInfo connectionInfo, string sftpFilePath)
        {
            return DisposableService.Using(() => new SftpClient(connectionInfo), client =>
            {
                if (null == client)
                    return null;
                client.Connect();
                return client.ListDirectory(sftpFilePath)?.ToList();



            });
        }

        public static ConnectionInfo GetConnectionInfo(string host, string userName, string password,
            int? port, string sftpFilePath)
        {
            var connectionInfo = null != port
                ? new ConnectionInfo(host, port.Value, userName, new PasswordAuthenticationMethod(userName, password))
                : new ConnectionInfo(host, userName, new PasswordAuthenticationMethod(userName, password));
            return connectionInfo;
        }
    }
}