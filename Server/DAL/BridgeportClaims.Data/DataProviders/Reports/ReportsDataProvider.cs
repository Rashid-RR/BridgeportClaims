using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.Reports
{
    public class ReportsDataProvider : IReportsDataProvider
    {
        public IList<AccountsReceivableDto> GetAccountsReceivableReport()
            => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                return DisposableService.Using(() => new SqlCommand("rpt.uspGetAccountsReceivable", conn), cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var startDateParam = cmd.CreateParameter();
                    startDateParam.Value = new DateTime(2017, 1, 1);
                    startDateParam.DbType = DbType.Date;
                    startDateParam.SqlDbType = SqlDbType.Date;
                    startDateParam.Direction = ParameterDirection.Input;
                    startDateParam.ParameterName = "@StartDate";
                    cmd.Parameters.Add(startDateParam);
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