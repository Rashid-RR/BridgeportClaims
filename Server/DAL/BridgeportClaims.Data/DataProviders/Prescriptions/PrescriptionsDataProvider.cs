using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Data.Enums;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Entities.DomainModels;
using Dapper;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.Prescriptions
{
    public class PrescriptionsDataProvider : IPrescriptionsDataProvider
    {
        private readonly Lazy<IRepository<Prescription>> _prescriptionRepository;
        private readonly Lazy<IRepository<PrescriptionStatus>> _prescriptionStatusRepository;

        public PrescriptionsDataProvider(Lazy<IRepository<Prescription>> prescriptionRepository, Lazy<IRepository<PrescriptionStatus>> prescriptionStatusRepository)
        {
            _prescriptionRepository = prescriptionRepository;
            _prescriptionStatusRepository = prescriptionStatusRepository;
        }

        public EntityOperation AddOrUpdatePrescriptionStatus(int prescriptionId, int prescriptionStatusId)
        {
            var prescription = _prescriptionRepository.Value.Get(prescriptionId);
            if (null == prescription)
                throw new ArgumentNullException(nameof(prescription));
            var prescriptionStatus = _prescriptionStatusRepository.Value.Get(prescriptionStatusId);
            var op = null == prescription.PrescriptionStatus ? EntityOperation.Add : EntityOperation.Update;
            prescription.PrescriptionStatus = prescriptionStatus ?? throw new ArgumentNullException(nameof(prescriptionStatus));
            prescription.UpdatedOnUtc = DateTime.UtcNow;
            _prescriptionRepository.Value.Update(prescription);
            return op;
        }

        public void ArchiveUnpaidScript(int prescriptionId, string userId) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                conn.Open();
                conn.Execute("[dbo].[uspUnpaidScriptsArchivedInsert]",
                    new {PrescriptionID = prescriptionId, ModifiedByUserID = userId},
                    commandType: CommandType.StoredProcedure);
            });

        public UnpaidScriptsMasterDto GetUnpaidScripts(bool isDefaultSort, DateTime? startDate, DateTime? endDate,
            string sort, string sortDirection, int page, int pageSize, bool isArchived, int? payorId)
            => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                var sp = !isArchived ? "[dbo].[uspGetUnpaidScripts]" : "[dbo].[uspGetArchivedUnpaidScripts]";
                conn.Open();
                var ps = new DynamicParameters();
                ps.Add("@IsDefaultSort", isDefaultSort, DbType.Boolean);
                ps.Add("@StartDate", startDate, DbType.Date);
                ps.Add("@EndDate", endDate, DbType.Date);
                ps.Add("@SortColumn", sort, DbType.AnsiString, ParameterDirection.Input, 50);
                ps.Add("@SortDirection", sortDirection, DbType.AnsiString, ParameterDirection.Input, 5);
                ps.Add("@PageNumber", page, DbType.Int32);
                ps.Add("@PageSize", pageSize, DbType.Int32);
                ps.Add("@PayorID", payorId, DbType.Int32);
                ps.Add("@TotalRows", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var reader = conn.QueryMultiple(sp, ps, commandType: CommandType.StoredProcedure);
                var unpaidScriptsDto = new UnpaidScriptsDto();
                var payors = reader.Read<PayorDto>()?.ToList();
                var unpaidScripts = reader.Read<UnpaidScriptResultDto>()?.ToList();
                unpaidScriptsDto.UnpaidScriptResults = unpaidScripts;
                unpaidScriptsDto.TotalRowCount = ps.Get<int>("@TotalRows");
                var master = new UnpaidScriptsMasterDto
                {
                    UnpaidScripts = unpaidScriptsDto,
                    Payors = payors?.OrderBy(x => x.Carrier).ToList()
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
                        prescriptionIdsParam.TypeName = "dbo.udtPrescriptionID";
                        prescriptionIdsParam.Direction = ParameterDirection.Input;
                        prescriptionIdsParam.Value = dtos.ToFixedDataTable();
                        prescriptionIdsParam.ParameterName = "@PrescriptionIDs";
                        cmd.Parameters.Add(prescriptionIdsParam);
                        IList<string> retVal = new List<string>();
                        if (conn.State != ConnectionState.Open)
                            conn.Open();
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
    }
}