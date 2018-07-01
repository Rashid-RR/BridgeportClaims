using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.Dtos;
using Dapper;
using NLog;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.Reports
{
    public class ReportsDataProvider : IReportsDataProvider
    {
        private static readonly Lazy<Logger> Logger = new Lazy<Logger>(LogManager.GetCurrentClassLogger);

        public SkippedPaymentDto GetSkippedPaymentReport(DataTable carriers)
            => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                conn.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@Carriers", carriers.AsTableValuedParameter("[dbo].[udtPayorID]"));
                parameters.Add("@TotalRowCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
                const string sp = "[rpt].[uspGetSkippedPayment]";
                var query = conn.Query<SkippedPaymentResultsDto>(sp, parameters, commandType: CommandType.StoredProcedure);
                var retVal = new SkippedPaymentDto
                {
                    Results = query?.ToList(),
                    TotalRowCount = parameters.Get<int>("@TotalRowCount")
                };
                return retVal;
            });

        private static string GetGroupNameSqlQuery(string groupName)
            => $@"DECLARE @GroupName VARCHAR(255) = '{groupName}'
                  SELECT p.GroupName FROM dbo.Payor AS p
                  WHERE  p.GroupName LIKE '%' + @GroupName + '%'";

        private static string GetPharmacyNameSqlQuery(string pharmacyName)
            => $@"DECLARE @PharmacyName VARCHAR(60) = '{pharmacyName}'
                  SELECT P.NABP, P.PharmacyName FROM dbo.Pharmacy AS p
                  WHERE p.PharmacyName LIKE '%' + @PharmacyName + '%'";

        public IList<PharmacyNameDto> GetPharmacyNames(string pharmacyName)
            => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                return DisposableService.Using(() => new SqlCommand(GetPharmacyNameSqlQuery(pharmacyName), conn), cmd =>
                {
                    cmd.CommandType = CommandType.Text;
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    return DisposableService.Using(cmd.ExecuteReader, reader =>
                    {
                        var pharmacyNameOrdinal = reader.GetOrdinal("PharmacyName");
                        var pharmacyNabpOrdinal = reader.GetOrdinal("NABP");
                        IList<PharmacyNameDto> retVal = new List<PharmacyNameDto>();
                        while (reader.Read())
                        {
                            var pharmacyNameDto = new PharmacyNameDto
                            {
                                Nabp = !reader.IsDBNull(pharmacyNabpOrdinal) ? reader.GetString(pharmacyNabpOrdinal) : string.Empty,
                                PharmacyName = !reader.IsDBNull(pharmacyNameOrdinal) ? reader.GetString(pharmacyNameOrdinal) : string.Empty
                            };
                            if (null != pharmacyNameDto.PharmacyName && pharmacyNameDto.PharmacyName.IsNotNullOrWhiteSpace())
                                retVal.Add(pharmacyNameDto);
                        }
                        return retVal.OrderBy(x => x.PharmacyName).ToList();
                    });
                });
            });

        public DuplicateClaimDto GetDuplicateClaims(string sort, string sortDirection, int page = -1, int pageSize = -1)
            => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                conn.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@SortColumn", sort, DbType.AnsiString);
                parameters.Add("@SortDirection", sortDirection, DbType.AnsiString);
                parameters.Add("@PageNumber", page == -1 ? 1 : page, DbType.Int32);
                parameters.Add("@PageSize", pageSize == -1 ? 5000 : pageSize, DbType.Int32);
                parameters.Add("@TotalRows", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var results = conn.Query<DuplicateClaimResultsDto>("[rpt].[uspGetDuplicateClaims]", parameters,
                    commandType: CommandType.StoredProcedure);
                var retVal = new DuplicateClaimDto
                {
                    Results = results?.ToList(),
                    TotalRowCount = parameters.Get<int>("@TotalRows")
                };
                return retVal;
            });

        public IList<GroupNameDto> GetGroupNames(string groupName)
            => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                return DisposableService.Using(() => new SqlCommand(GetGroupNameSqlQuery(groupName), conn), cmd =>
                {
                    cmd.CommandType = CommandType.Text;
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    return DisposableService.Using(cmd.ExecuteReader, reader =>
                    {
                        var groupNameOrdinal = reader.GetOrdinal("GroupName");
                        IList<GroupNameDto> retVal = new List<GroupNameDto>();
                        while (reader.Read())
                        {
                            var groupNameDto = new GroupNameDto
                            {
                                GroupName = !reader.IsDBNull(groupNameOrdinal) ? reader.GetString(groupNameOrdinal) : string.Empty
                            };
                            if (null != groupNameDto.GroupName && groupNameDto.GroupName.IsNotNullOrWhiteSpace())
                                retVal.Add(groupNameDto);
                        }
                        return retVal.OrderBy(x => x.GroupName).ToList();
                    });
                });
            });

        public IList<AccountsReceivableDto> GetAccountsReceivableReport(string groupName, string pharmacyName)
            => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                return DisposableService.Using(() => new SqlCommand("rpt.uspGetAccountsReceivable", conn), cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var groupNameParam = cmd.CreateParameter();
                    groupNameParam.Direction = ParameterDirection.Input;
                    groupNameParam.DbType = DbType.AnsiString;
                    groupNameParam.SqlDbType = SqlDbType.VarChar;
                    groupNameParam.Size = 60;
                    groupNameParam.ParameterName = "@GroupName";
                    groupNameParam.Value = groupName.IsNotNullOrWhiteSpace() ? groupName : (object) DBNull.Value;
                    cmd.Parameters.Add(groupNameParam);
                    var pharmacyNameParam = cmd.CreateParameter();
                    pharmacyNameParam.Direction = ParameterDirection.Input;
                    pharmacyNameParam.Value = pharmacyName.IsNotNullOrWhiteSpace() ? pharmacyName : (object) DBNull.Value;
                    pharmacyNameParam.DbType = DbType.AnsiString;
                    pharmacyNameParam.SqlDbType = SqlDbType.VarChar;
                    pharmacyNameParam.Size = 255;
                    pharmacyNameParam.ParameterName = "@PharmacyName";
                    cmd.Parameters.Add(pharmacyNameParam);
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    return DisposableService.Using(cmd.ExecuteReader, reader =>
                    {
                        var monthBilledOrdinal = reader.GetOrdinal("DateBilled");
                        var totalInvoicedOrdinal = reader.GetOrdinal("TotalInvoiced");
                        var jan17Ordinal = reader.GetOrdinal("Mnth1");
                        var feb17Ordinal = reader.GetOrdinal("Mnth2");
                        var mar17Ordinal = reader.GetOrdinal("Mnth3");
                        var apr17Ordinal = reader.GetOrdinal("Mnth4");
                        var may17Ordinal = reader.GetOrdinal("Mnth5");
                        var jun17Ordinal = reader.GetOrdinal("Mnth6");
                        var jul17Ordinal = reader.GetOrdinal("Mnth7");
                        var aug17Ordinal = reader.GetOrdinal("Mnth8");
                        var sep17Ordinal = reader.GetOrdinal("Mnth9");
                        var oct17Ordinal = reader.GetOrdinal("Mnth10");
                        var nov17Ordinal = reader.GetOrdinal("Mnth11");
                        var dec17Ordinal = reader.GetOrdinal("Mnth12");
                        var retVal = new List<AccountsReceivableDto>();
                        while (reader.Read())
                        {
                            var record = new AccountsReceivableDto
                            {
                                DateBilled = reader.GetString(monthBilledOrdinal),
                                TotalInvoiced = reader.GetDecimal(totalInvoicedOrdinal),
                                Mnth1 = reader.GetDecimal(jan17Ordinal),
                                Mnth2 = reader.GetDecimal(feb17Ordinal),
                                Mnth3 = reader.GetDecimal(mar17Ordinal),
                                Mnth4 = reader.GetDecimal(apr17Ordinal),
                                Mnth5 = reader.GetDecimal(may17Ordinal),
                                Mnth6 = reader.GetDecimal(jun17Ordinal),
                                Mnth7 = reader.GetDecimal(jul17Ordinal),
                                Mnth8 = reader.GetDecimal(aug17Ordinal),
                                Mnth9 = reader.GetDecimal(sep17Ordinal),
                                Mnth10 = reader.GetDecimal(oct17Ordinal),
                                Mnth11 = reader.GetDecimal(nov17Ordinal),
                                Mnth12 = reader.GetDecimal(dec17Ordinal)
                            };
                            retVal.Add(record);
                        }
                        return retVal;
                    });
                });
            });

        public ShortPayDto GetShortPayReport(string sort, string sortDirection, int page,
            int pageSize) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                try
                {
                    const string sp = "[rpt].[uspGetShortpayReport]";
                    conn.Open();
                    var parameters = new DynamicParameters();
                    parameters.Add("@SortColumn", sort, DbType.AnsiString);
                    parameters.Add("@SortDirection", sortDirection, DbType.AnsiString);
                    parameters.Add("@PageNumber", page == -1 ? 1 : page, DbType.Int32);
                    parameters.Add("@PageSize", pageSize == -1 ? 5000 : pageSize, DbType.Int32);
                    parameters.Add("@TotalRowCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    var results = conn.Query<ShortPayResultDto>(sp, commandType: CommandType.StoredProcedure, param: parameters);
                    var retVal = new ShortPayDto
                    {
                        Results = results?.ToList(),
                        TotalRowCount = parameters.Get<int>("@TotalRowCount")
                    };
                    return retVal;
                }
                catch (Exception ex)
                {
                    Logger.Value.Error(ex);
                    throw;
                }
            });

        public bool RemoveShortPay(int prescriptionPaymentId, string userId)
            => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                try
                {
                    var cmd = "INSERT dbo.ShortPayExclusion (PrescriptionPaymentID, ModifiedByUserID)" +
                              $"VALUES({prescriptionPaymentId}, '{userId}')";
                    conn.Open();
                    conn.Execute(cmd, commandType: CommandType.Text);
                    return true;
                }
                catch (Exception ex)
                {
                    Logger.Value.Error(ex);
                    return false;
                }
            });
    }
}