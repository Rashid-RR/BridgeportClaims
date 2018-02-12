using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.AdminFunctions
{
    public interface IAdminFunctionsProvider
    {
        void DeleteFirewallSetting(string ruleName);
        void AddFirewallSetting(string ruleName, string startIpAddress, string endIpAddress);
        IList<FirewallSetting> GetFirewallSettings();
    }
}