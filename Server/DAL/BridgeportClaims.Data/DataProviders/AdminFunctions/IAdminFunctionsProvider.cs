using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.AdminFunctions
{
    public interface IAdminFunctionsProvider
    {
        IList<FirewallSetting> GetFirewallSettings();
    }
}