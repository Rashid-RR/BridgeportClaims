using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.Reports
{
    public interface IReportsDataProvider
    {
        IList<PharmacyNameDto> GetPharmacyNames(string pharmacyName);
        DuplicateClaimDto GetDuplicateClaims(string sort, string sortDirection, int page, int pageSize);
        IList<GroupNameDto> GetGroupNames(string groupName);
        IList<AccountsReceivableDto> GetAccountsReceivableReport(string groupName, string pharmacyName);
        ShortPayDto GetShortPayReport(string sort, string sortDirection, int pageNumber, int pageSize);
    }
}