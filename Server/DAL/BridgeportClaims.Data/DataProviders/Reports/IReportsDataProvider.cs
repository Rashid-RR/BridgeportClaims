using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.Reports
{
    public interface IReportsDataProvider
    {
        IList<PharmacyNameDto> GetPharmacyNames(string pharmacyName);
        IList<DuplicateClaimDto> GetDuplicateClaims();
        IList<GroupNameDto> GetGroupNames(string groupName);
        IList<AccountsReceivableDto> GetAccountsReceivableReport(string groupName, string pharmacyName);
    }
}