using cs = LakerFileImporter.ConfigService.ConfigService;
using c = LakerFileImporter.StringConstants.Constants;

namespace LakerFileImporter.Security
{
    internal class CompiledSecurityProvider
    {
        internal string
            RawConnectionString // => "Server=jdgdb1.database.windows.net;Database=BridgeportClaims;User ID=bridgeportclaimslakerimporter;Password=v9fy!QBRM@;Trusted_Connection=False;";
            => @"Server=.\DB1;Database=BridgeportClaims;Trusted_Connection=True;";

        internal string RawBridgeportClaimsSiteUserPassword => cs.GetAppSetting(c.AuthenticationPasswordKey);

        internal string RawLakerSftpPassword => "B171!or11";

        internal string RawEnvisionSftpPassword = "jvD4EHQ#";
    }
}
