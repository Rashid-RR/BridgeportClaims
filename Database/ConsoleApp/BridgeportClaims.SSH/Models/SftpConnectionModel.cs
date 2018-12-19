using System;

namespace BridgeportClaims.SSH.Models
{
    [Serializable]
    public sealed class SftpConnectionModel
    {
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RemoteSitePath { get; set; }
        public int? Port { get; set; }
    }
}