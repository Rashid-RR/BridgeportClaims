using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using Dapper;
using SQLinq;
using SQLinq.Dapper;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.AdjustorSearches
{
    public class AdjustorSearchProvider : IAdjustorSearchProvider
    {
        public IList<AdjustorSearchResultsDto> GetAdjustorSearchResults(string searchText) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                return DisposableService.Using(() => new SqlCommand("[dbo].[uspAdjustorTextSearch]", conn), cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var searchTextParam = cmd.CreateParameter();
                    searchTextParam.Value = searchText ?? (object) DBNull.Value;
                    searchTextParam.ParameterName = "@SearchText";
                    searchTextParam.DbType = DbType.AnsiString;
                    searchTextParam.SqlDbType = SqlDbType.VarChar;
                    searchTextParam.Size = 800;
                    searchTextParam.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(searchTextParam);
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    return DisposableService.Using(cmd.ExecuteReader, reader =>
                    {
                        IList<AdjustorSearchResultsDto> retVal = new List<AdjustorSearchResultsDto>();
                        var adjustorIdOrdinal = reader.GetOrdinal("AdjustorId");
                        var adjustorNameOrdinal = reader.GetOrdinal("AdjustorName");
                        while (reader.Read())
                        {
                            var result = new AdjustorSearchResultsDto
                            {
                                AdjustorId = reader.GetInt32(adjustorIdOrdinal),
                                AdjustorName = reader.GetString(adjustorNameOrdinal)
                            };
                            retVal.Add(result);
                        }
                        if (conn.State != ConnectionState.Closed)
                            conn.Close();
                        return retVal.OrderBy(x => x.AdjustorName).ToList();
                    });
                });
            });

        public IEnumerable<AdjustorNameDto> GetAdjustorNames(string adjustorName)
            => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                conn.Open();
                return conn.Query(new SQLinq<AdjustorNameDto>()
                    .Where(p => p.AdjustorName.Contains(adjustorName))
                    .Select(p => new {p.AdjustorId, p.AdjustorName}));
            });
    }
}