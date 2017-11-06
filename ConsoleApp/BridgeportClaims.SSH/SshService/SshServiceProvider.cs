using BridgeportClaims.SSH.Disposable;
using Renci.SshNet;

namespace BridgeportClaims.SSH.SshService
{
    public class SshServiceProvider
    {
        public void TraverseSshDirectory(string host, string userName, string password, string sftpFilePath)
        {
            var connectionInfo = new ConnectionInfo(host, 22, userName, new PasswordAuthenticationMethod(userName, password));
            DisposableService.Using(() => new SftpClient(connectionInfo), client =>
            {
                client.Connect();
                var directory =  client.ListDirectory(sftpFilePath);
                client.Disconnect();
            });
        }
    }
}