using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.Dtos;
using Dapper;
using SQLinq;
using SQLinq.Dapper;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.AdjustorSearches
{
    public class AdjustorDataProvider : IAdjustorDataProvider
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
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                if (adjustorName.IsNotNullOrWhiteSpace())
                {
                    return conn.Query(new SQLinq<AdjustorNameDto>()
                        .Where(p => p.AdjustorName.Contains(adjustorName))
                        .OrderBy(p => p.AdjustorName)
                        .Select(p => new {p.AdjustorId, p.AdjustorName}));
                }
                return conn.Query(new SQLinq<AdjustorNameDto>()
                    .OrderBy(p => p.AdjustorName)
                    .Select(p => new {p.AdjustorId, p.AdjustorName}));
            });

        public AdjustorDto GetAdjustors(string searchText, int page, int pageSize, string sort, string sortDirection)
            => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[dbo].[uspGetReferencesAdjustors]";
                const string totalRows = "@TotalRows";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                var ps = new DynamicParameters();
                ps.Add("@SearchText", searchText, DbType.AnsiString, size: 4000);
                ps.Add("@SortColumn", sort, DbType.AnsiString, size: 50);
                ps.Add("@SortDirection", sortDirection, DbType.AnsiString, size: 5);
                ps.Add("@PageNumber", page, DbType.Int32);
                ps.Add("@PageSize", pageSize, DbType.Int32);
                ps.Add(totalRows, DbType.Int32, direction: ParameterDirection.Output);
                var query = conn.Query<AdjustorResultDto>(sp, ps, commandType: CommandType.StoredProcedure);
                var adj = new AdjustorDto
                {
                    Results = query,
                    TotalRows = ps.Get<int>(totalRows)
                };
                return adj;
            });

        public AdjustorResultDto InsertAdjustor(string adjustorName, string address1, string address2,
            string city, int? stateId, string postalCode, string phoneNumber,
            string faxNumber, string emailAddress, string extension, string modifiedByUserId)
            => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[claims].[uspAdjustorInsert]";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                var ps = new DynamicParameters();
                ps.Add("@AdjustorName", adjustorName, DbType.AnsiString, size: 255);
                ps.Add("@Address1", address1, DbType.AnsiString, size: 255);
                ps.Add("@Address2", address2, DbType.AnsiString, size: 255);
                ps.Add("@City", city, DbType.AnsiString, size: 255);
                ps.Add("@StateID", stateId, DbType.Int32);
                ps.Add("@PostalCode", postalCode, DbType.AnsiString, size: 255);
                ps.Add("@PhoneNumber", phoneNumber, DbType.AnsiString, size: 30);
                ps.Add("@FaxNumber", faxNumber, DbType.AnsiString, size: 30);
                ps.Add("@EmailAddress", emailAddress, DbType.AnsiString, size: 155);
                ps.Add("@Extension", extension, DbType.AnsiString, size: 10);
                ps.Add("@ModifiedByUserID", modifiedByUserId, DbType.String, size: 128);
                return conn.Query<AdjustorResultDto>(sp, ps, commandType: CommandType.StoredProcedure)
                    ?.SingleOrDefault();
            });

        public AdjustorResultDto UpdateAdjustor(int adjustorId, string adjustorName, string address1,
            string address2, string city, int? stateId, string postalCode, string phoneNumber,
            string faxNumber, string emailAddress, string extension, string modifiedByUserId)
            => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[claims].[uspAdjustorUpdate]";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                var ps = new DynamicParameters();
                ps.Add("@AdjustorID", adjustorId, DbType.Int32);
                ps.Add("@AdjustorName", adjustorName, DbType.AnsiString, size: 255);
                ps.Add("@Address1", address1, DbType.AnsiString, size: 255);
                ps.Add("@Address2", address2, DbType.AnsiString, size: 255);
                ps.Add("@City", city, DbType.AnsiString, size: 255);
                ps.Add("@StateID", stateId, DbType.Int32);
                ps.Add("@PostalCode", postalCode, DbType.AnsiString, size: 255);
                ps.Add("@PhoneNumber", phoneNumber, DbType.AnsiString, size: 30);
                ps.Add("@FaxNumber", faxNumber, DbType.AnsiString, size: 30);
                ps.Add("@EmailAddress", emailAddress, DbType.AnsiString, size: 155);
                ps.Add("@Extension", extension, DbType.AnsiString, size: 10);
                ps.Add("@ModifiedByUserID", modifiedByUserId, DbType.String, size: 128);
                return conn.Query<AdjustorResultDto>(sp, ps, commandType: CommandType.StoredProcedure)
                    ?.SingleOrDefault();
            });
    }
}