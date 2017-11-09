using System.Collections.Generic;
using System.Linq;
using BridgeportClaims.SSH.Disposable;
using Renci.SshNet;
using Renci.SshNet.Sftp;

namespace BridgeportClaims.SSH.SshService
{
    public class SshServiceProvider
    {
        public IList<SftpFile> TraverseSshDirectory(string host, string userName, string password, 
            int? port, string sftpFilePath)
        {
            var connectionInfo = null != port
                ? new ConnectionInfo(host, port.Value, userName, new PasswordAuthenticationMethod(userName, password))
                : new ConnectionInfo(host, userName, new PasswordAuthenticationMethod(userName, password));
            return DisposableService.Using(() => new SftpClient(connectionInfo), client =>
            {
                if (null == client)
                    return null;
                client.Connect();
                return client.ListDirectory(sftpFilePath)?.ToList();
            });
        }
    }
}