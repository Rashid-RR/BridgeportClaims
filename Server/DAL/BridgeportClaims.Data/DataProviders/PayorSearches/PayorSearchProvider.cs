using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.PayorSearches
{
    public class PayorSearchProvider : IPayorSearchProvider
    {
        public IList<PayorSearchResultsDto> GetPayorSearchResults(string searchText) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                return DisposableService.Using(() => new SqlCommand("[dbo].[uspPayorTextSearch]", conn), cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    var searchTextParam = cmd.CreateParameter();
                    searchTextParam.Direction = ParameterDirection.Input;
                    searchTextParam.Size = 800;
                    searchTextParam.DbType = DbType.AnsiString;
                    searchTextParam.SqlDbType = SqlDbType.VarChar;
                    searchTextParam.Value = searchText ?? (object) DBNull.Value;
                    searchTextParam.ParameterName = "@SearchText";
                    cmd.Parameters.Add(searchTextParam);
                    return DisposableService.Using(cmd.ExecuteReader, reader =>
                    {
                        IList<PayorSearchResultsDto> retVal = new List<PayorSearchResultsDto>();
                        var payorIdOrdinal = reader.GetOrdinal("PayorId");
                        var groupNameOrdinal = reader.GetOrdinal("GroupName");
                        while (reader.Read())
                        {
                            var result = new PayorSearchResultsDto
                            {
                                PayorId = reader.GetInt32(payorIdOrdinal),
                                GroupName = reader.GetString(groupNameOrdinal)
                            };
                            retVal.Add(result);
                        }
                        if (conn.State != ConnectionState.Closed)
                            conn.Close();
                        return retVal.OrderBy(x => x.GroupName).ToList();
                    });
                });
            });
    }
}