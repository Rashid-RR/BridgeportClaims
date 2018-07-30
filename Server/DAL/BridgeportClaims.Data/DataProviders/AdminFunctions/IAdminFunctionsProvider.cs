using System;
using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.AdminFunctions
{
    public interface IAdminFunctionsProvider
    {
        IEnumerable<InvoiceAmountDto> GetInvoiceAmounts(int claimId, string rxNumber,
            DateTime? rxDate = null, string invoiceNumber = null);
        void DeleteFirewallSetting(string ruleName);
        void AddFirewallSetting(string ruleName, string startIpAddress, string endIpAddress);
        IList<FirewallSetting> GetFirewallSettings();
        void UpdateBilledAmount(int prescriptionId, decimal billedAmount);
    }
}