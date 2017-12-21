using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.Dtos;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.Reports
{
    public class ReportsDataProvider : IReportsDataProvider
    {
        private static string GetGroupNameSqlQuery(string groupName)
            => $@"DECLARE @GroupName VARCHAR(255) = '{groupName}'
                  SELECT p.GroupName FROM dbo.Payor AS p
                  WHERE  p.GroupName LIKE '%' + @GroupName + '%'";

        private static string GetPharmacyNameSqlQuery(string pharmacyName)
            => $@"DECLARE @PharmacyName VARCHAR(60) = '{pharmacyName}'
                  SELECT P.PharmacyName FROM dbo.Pharmacy AS p
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
                        IList<PharmacyNameDto> retVal = new List<PharmacyNameDto>();
                        while (reader.Read())
                        {
                            var pharmacyNameDto = new PharmacyNameDto
                            {
                                PharmacyName = !reader.IsDBNull(pharmacyNameOrdinal) ? reader.GetString(pharmacyNameOrdinal) : string.Empty
                            };
                            if (null != pharmacyNameDto.PharmacyName && pharmacyNameDto.PharmacyName.IsNotNullOrWhiteSpace())
                                retVal.Add(pharmacyNameDto);
                        }
                        return retVal.OrderBy(x => x.PharmacyName).ToList();
                    });
                });
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
                    var startDateParam = cmd.CreateParameter();
                    startDateParam.Value = new DateTime(DateTime.Now.Year, 1, 1);
                    startDateParam.DbType = DbType.Date;
                    startDateParam.SqlDbType = SqlDbType.Date;
                    startDateParam.Direction = ParameterDirection.Input;
                    startDateParam.ParameterName = "@StartDate";
                    cmd.Parameters.Add(startDateParam);
                    var endDateParam = cmd.CreateParameter();
                    endDateParam.Direction = ParameterDirection.Input;
                    endDateParam.DbType = DbType.Date;
                    endDateParam.SqlDbType = SqlDbType.Date;
                    endDateParam.Value = new DateTime(DateTime.Now.Year, 12, 1);
                    endDateParam.ParameterName = "@EndDate";
                    cmd.Parameters.Add(endDateParam);
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
                        var monthBilledOrdinal = reader.GetOrdinal("MonthBilled");
                        var totalInvoicedOrdinal = reader.GetOrdinal("TotalInvoiced");
                        var jan17Ordinal = reader.GetOrdinal("Jan17");
                        var feb17Ordinal = reader.GetOrdinal("Feb17");
                        var mar17Ordinal = reader.GetOrdinal("Mar17");
                        var apr17Ordinal = reader.GetOrdinal("Apr17");
                        var may17Ordinal = reader.GetOrdinal("May17");
                        var jun17Ordinal = reader.GetOrdinal("Jun17");
                        var jul17Ordinal = reader.GetOrdinal("Jul17");
                        var aug17Ordinal = reader.GetOrdinal("Aug17");
                        var sep17Ordinal = reader.GetOrdinal("Sep17");
                        var oct17Ordinal = reader.GetOrdinal("Oct17");
                        var nov17Ordinal = reader.GetOrdinal("Nov17");
                        var dec17Ordinal = reader.GetOrdinal("Dec17");
                        var retVal = new List<AccountsReceivableDto>();
                        while (reader.Read())
                        {
                            var record = new AccountsReceivableDto
                            {
                                MonthBilled = reader.GetString(monthBilledOrdinal),
                                TotalInvoiced = reader.GetDecimal(totalInvoicedOrdinal),
                                Jan17 = reader.GetDecimal(jan17Ordinal),
                                Feb17 = reader.GetDecimal(feb17Ordinal),
                                Mar17 = reader.GetDecimal(mar17Ordinal),
                                Apr17 = reader.GetDecimal(apr17Ordinal),
                                May17 = reader.GetDecimal(may17Ordinal),
                                Jun17 = reader.GetDecimal(jun17Ordinal),
                                Jul17 = reader.GetDecimal(jul17Ordinal),
                                Aug17 = reader.GetDecimal(aug17Ordinal),
                                Sep17 = reader.GetDecimal(sep17Ordinal),
                                Oct17 = reader.GetDecimal(oct17Ordinal),
                                Nov17 = reader.GetDecimal(nov17Ordinal),
                                Dec17 = reader.GetDecimal(dec17Ordinal)
                            };
                            retVal.Add(record);
                        }
                        return retVal;
                    });
                });
            });
    }
}