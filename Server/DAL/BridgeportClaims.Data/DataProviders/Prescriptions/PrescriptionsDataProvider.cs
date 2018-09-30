using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Data.Enums;
using Dapper;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.Prescriptions
{
    public class PrescriptionsDataProvider : IPrescriptionsDataProvider
    {
        public EntityOperation AddOrUpdatePrescriptionStatus(int prescriptionId, 
            int prescriptionStatusId, string userId)
            => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[claims].[uspAddOrUpdatePrescriptionStatus]";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                var ps = new DynamicParameters();
                ps.Add("@PrescriptionID", prescriptionId, DbType.Int32);
                ps.Add("@PrescriptionStatusID", prescriptionStatusId, DbType.Int32);
                ps.Add("@ModifiedByUserID", userId, DbType.String, ParameterDirection.Input, 128);
                ps.Add("@Add", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                conn.Execute(sp, ps, commandType: CommandType.StoredProcedure);
                var add = ps.Get<bool>("@Add");
                var op = add ? EntityOperation.Add : EntityOperation.Update;
                return op;
            });

        public void ArchiveUnpaidScript(int prescriptionId, string userId) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                conn.Open();
                conn.Execute("[dbo].[uspUnpaidScriptsArchivedInsert]",
                    new {PrescriptionID = prescriptionId, ModifiedByUserID = userId},
                    commandType: CommandType.StoredProcedure);
            });

        public UnpaidScriptsMasterDto GetUnpaidScripts(bool isDefaultSort, DateTime? startDate, DateTime? endDate,
            string sort, string sortDirection, int page, int pageSize, bool isArchived, DataTable carriers)
            => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                var sp = !isArchived ? "[dbo].[uspGetUnpaidScripts]" : "[dbo].[uspGetArchivedUnpaidScripts]";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                var ps = new DynamicParameters();
                ps.Add("@IsDefaultSort", isDefaultSort, DbType.Boolean);
                ps.Add("@StartDate", startDate, DbType.Date);
                ps.Add("@EndDate", endDate, DbType.Date);
                ps.Add("@SortColumn", sort, DbType.AnsiString, ParameterDirection.Input, 50);
                ps.Add("@SortDirection", sortDirection, DbType.AnsiString, ParameterDirection.Input, 5);
                ps.Add("@PageNumber", page, DbType.Int32);
                ps.Add("@PageSize", pageSize, DbType.Int32);
                ps.Add("@Carriers", carriers.AsTableValuedParameter("[dbo].[udtID]"));
                ps.Add("@TotalRows", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var unpaidScripts = conn.Query<UnpaidScriptResultDto>(sp, ps, commandType: CommandType.StoredProcedure);
                var unpaidScriptsDto = new UnpaidScriptsDto
                {
                    UnpaidScriptResults = unpaidScripts?.ToList(),
                    TotalRowCount = ps.Get<int>("@TotalRows")
                };
                var master = new UnpaidScriptsMasterDto
                {
                    UnpaidScripts = unpaidScriptsDto
                };
                return master;
            });
        
        public IEnumerable<string> GetFileUrlsFromPrescriptionIds(IEnumerable<PrescriptionIdDto> dtos) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
                {
                    if (null == dtos)
                        throw new ArgumentNullException(nameof(dtos));
                    return DisposableService.Using(() => new SqlCommand("[dbo].[uspGetInvoiceUrlsFromPrescriptionIDs]", conn), cmd =>
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        var prescriptionIdsParam = cmd.CreateParameter();
                        prescriptionIdsParam.SqlDbType = SqlDbType.Structured;
                        prescriptionIdsParam.TypeName = "[dbo].[udtID]";
                        prescriptionIdsParam.Direction = ParameterDirection.Input;
                        prescriptionIdsParam.Value = dtos.ToFixedDataTable();
                        prescriptionIdsParam.ParameterName = "@PrescriptionIDs";
                        cmd.Parameters.Add(prescriptionIdsParam);
                        IList<string> retVal = new List<string>();
                        if (conn.State != ConnectionState.Open)
                        {
                            conn.Open();
                        }
                        DisposableService.Using(cmd.ExecuteReader, reader =>
                        {
                            while (reader.Read())
                            {
                                var invoiceUrlOrdinal = reader.GetOrdinal("InvoiceUrl");
                                var result = reader.GetString(invoiceUrlOrdinal);
                                retVal.Add(result);
                            }
                        });
                        return retVal.AsEnumerable();
                    });
                });

        public void SetMultiplePrescriptionStatuses(DataTable dt, int prescriptionStatusId, string userId)
            => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                const string sp = "[dbo].[uspSaveMultiplePrescriptionStatuses]";
                var ps = new DynamicParameters();
                ps.Add("@Prescription", dt.AsTableValuedParameter("[dbo].[udtID]"));
                ps.Add("@PrescriptionStatusID", prescriptionStatusId, DbType.Int32);
                ps.Add("@UserID", userId, DbType.String, ParameterDirection.Input, 128);
                conn.Execute(sp, ps, commandType: CommandType.StoredProcedure);
            });
    }
}