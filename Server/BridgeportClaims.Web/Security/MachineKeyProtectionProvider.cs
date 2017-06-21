using Microsoft.Owin.Security.DataProtection;

namespace BridgeportClaims.Web.Security
{
    internal class MachineKeyProtectionProvider : IDataProtectionProvider
    {
        public IDataProtector Create(params string[] purposes) => new MachineKeyDataProtector(purposes);
    }
}