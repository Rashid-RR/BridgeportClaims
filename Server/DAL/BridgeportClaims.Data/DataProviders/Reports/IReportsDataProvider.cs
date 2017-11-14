using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.Reports
{
    public interface IReportsDataProvider
    {
        IList<AccountsReceivableDto> GetAccountsReceivableReport();
    }
}