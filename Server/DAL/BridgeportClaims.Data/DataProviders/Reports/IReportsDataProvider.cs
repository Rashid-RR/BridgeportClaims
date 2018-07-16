using System.Collections.Generic;
using System.Data;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.Reports
{
    public interface IReportsDataProvider
    {
        SkippedPaymentDto GetSkippedPaymentReport(int page, int pageSize, DataTable carriers, bool archived);
        IList<PharmacyNameDto> GetPharmacyNames(string pharmacyName);
        DuplicateClaimDto GetDuplicateClaims(string sort, string sortDirection, int page, int pageSize);
        IList<GroupNameDto> GetGroupNames(string groupName);
        IList<AccountsReceivableDto> GetAccountsReceivableReport(string groupName, string pharmacyName);
        ShortPayDto GetShortPayReport(string sort, string sortDirection, int pageNumber, int pageSize);
        bool RemoveShortPay(int prescriptionId, string userId);
        bool RemoveSkippedPayment(int prescriptionId, string userId);
    }
}