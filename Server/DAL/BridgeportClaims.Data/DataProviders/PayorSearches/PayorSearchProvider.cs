using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SQLinq;
using SQLinq.Dapper;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.Dtos;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.PayorSearches
{
    public class PayorSearchProvider : IPayorSearchProvider
    {
        public IEnumerable<PayorSearchResultsDto> GetPayorSearchResults(string searchText) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                if (searchText.IsNotNullOrWhiteSpace())
                {
                    return conn.Query(new SQLinq<PayorSearchResultsDto>()
                        .Where(p => p.GroupName.Contains(searchText))
                        .OrderBy(p => p.GroupName)
                        .Select(p => new {p.PayorId, p.GroupName}));
                }
                return conn.Query(new SQLinq<PayorSearchResultsDto>()
                    .OrderBy(p => p.GroupName)
                    .Select(p => new {p.PayorId, p.GroupName}));
            });
    }
}