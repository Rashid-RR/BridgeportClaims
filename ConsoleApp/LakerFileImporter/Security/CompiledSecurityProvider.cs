using cs = LakerFileImporter.ConfigService.ConfigService;
using c = LakerFileImporter.StringConstants.Constants;

namespace LakerFileImporter.Security
{
    internal class CompiledSecurityProvider
    {
        internal string RawConnectionString
            => "Server=jdgdb1.database.windows.net;Database=BridgeportClaims;User ID=bridgeportclaimslakerimporter;Password=v9fy!QBRM@;Trusted_Connection=False;";

        internal string RawBridgeportClaimsSiteUserPassword => cs.GetAppSetting(c.AuthenticationPasswordKey);

        internal string RawLakerSftpPassword => "B171!or11";
    }
}
