using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Data.Enums;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Entities.DomainModels;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.Prescriptions
{
    [System.Runtime.InteropServices.Guid("0028E4FF-0EE4-44FA-AF9C-3AFE551D7CAE")]
    public class PrescriptionsProvider : IPrescriptionsProvider
    {
        private readonly IRepository<Prescription> _prescriptionRepository;
        private readonly IRepository<PrescriptionStatus> _prescriptionStatusRepository;

        public PrescriptionsProvider(IRepository<Prescription> prescriptionRepository, IRepository<PrescriptionStatus> prescriptionStatusRepository)
        {
            _prescriptionRepository = prescriptionRepository;
            _prescriptionStatusRepository = prescriptionStatusRepository;
        }

        public EntityOperation AddOrUpdatePrescriptionStatus(int prescriptionId, int prescriptionStatusId)
        {
            var prescription = _prescriptionRepository.Get(prescriptionId);
            if (null == prescription)
                throw new ArgumentNullException(nameof(prescription));
            var prescriptionStatus = _prescriptionStatusRepository.Get(prescriptionStatusId);
            var op = null == prescription.PrescriptionStatus ? EntityOperation.Add : EntityOperation.Update;
            prescription.PrescriptionStatus = prescriptionStatus ?? throw new ArgumentNullException(nameof(prescriptionStatus));
            prescription.UpdatedOnUtc = DateTime.UtcNow;
            _prescriptionRepository.Update(prescription);
            return op;
        }

        public UnpaidScriptsDto GetUnpaidScripts(bool isDefaultSort, DateTime? startDate, DateTime? endDate,
            string sort, string sortDirection, int page, int pageSize)
        {
            return DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {

                return DisposableService.Using(() => new SqlCommand("[dbo].[uspGetUnpaidScripts]", conn), cmd =>
                {
                    var isDefaultSortParam = cmd.CreateParameter();
                    isDefaultSortParam.ParameterName = "@IsDefaultSort";
                    isDefaultSortParam.Value = isDefaultSort;
                    isDefaultSortParam.DbType = DbType.Boolean;
                    isDefaultSortParam.SqlDbType = SqlDbType.Bit;
                    isDefaultSortParam.Direction = ParameterDirection.Input;
                    var startDateParam = cmd.CreateParameter();
                    startDateParam.ParameterName = "@StartDate";
                    startDateParam.DbType = DbType.Date;
                    startDateParam.Value = startDate ?? (object) DBNull.Value;
                    startDateParam.SqlDbType = SqlDbType.Date;
                    startDateParam.Direction = ParameterDirection.Input;
                    var endDateParam = cmd.CreateParameter();
                    endDateParam.ParameterName = "@EndDate";
                    endDateParam.Value = endDate ?? (object) DBNull.Value;
                    endDateParam.DbType = DbType.Date;
                    endDateParam.Direction = ParameterDirection.Input;
                    endDateParam.SqlDbType = SqlDbType.Date;
                    var sortParam = cmd.CreateParameter();
                    sortParam.ParameterName = "@SortColumn";
                    sortParam.DbType = DbType.String;
                    sortParam.Value = sort;
                    sortParam.Direction = ParameterDirection.Input;
                    sortParam.SqlDbType = SqlDbType.NVarChar;
                    var sortDirectionParam = cmd.CreateParameter();
                    sortDirectionParam.ParameterName = "@SortDirection";
                    sortDirectionParam.DbType = DbType.String;
                    sortDirectionParam.Value = sortDirection;
                    sortDirectionParam.SqlDbType = SqlDbType.VarChar;
                    sortDirectionParam.Direction = ParameterDirection.Input;
                    var pageParam = cmd.CreateParameter();
                    pageParam.ParameterName = "@PageNumber";
                    pageParam.DbType = DbType.Int32;
                    pageParam.Value = page;
                    pageParam.Direction = ParameterDirection.Input;
                    pageParam.SqlDbType = SqlDbType.Int;
                    var pageSizeParam = cmd.CreateParameter();
                    pageSizeParam.ParameterName = "@PageSize";
                    pageSizeParam.Value = pageSize;
                    pageSizeParam.DbType = DbType.Int32;
                    pageSizeParam.SqlDbType = SqlDbType.Int;
                    var totalRowsParam = cmd.CreateParameter();
                    totalRowsParam.ParameterName = "@TotalRows";
                    totalRowsParam.DbType = DbType.Int32;
                    totalRowsParam.SqlDbType = SqlDbType.Int;
                    totalRowsParam.Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(isDefaultSortParam);
                    cmd.Parameters.Add(startDateParam);
                    cmd.Parameters.Add(endDateParam);
                    cmd.Parameters.Add(sortParam);
                    cmd.Parameters.Add(sortDirectionParam);
                    cmd.Parameters.Add(pageParam);
                    cmd.Parameters.Add(pageSizeParam);
                    cmd.Parameters.Add(totalRowsParam);
                    IList<UnpaidScriptResultDto> retValResults = new List<UnpaidScriptResultDto>();
                    var retVal = new UnpaidScriptsDto();
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    DisposableService.Using(cmd.ExecuteReader, reader =>
                    {
                        
                        var prescriptionIdOrdinal = reader.GetOrdinal("PrescriptionId");
                        var claimIdOrdinal = reader.GetOrdinal("ClaimId");
                        var patientNameOrdinal = reader.GetOrdinal("PatientName");
                        var claimNumberOrdinal = reader.GetOrdinal("ClaimNumber");
                        var invoiceNumberOrdinal = reader.GetOrdinal("InvoiceNumber");
                        var invDateOrdinal = reader.GetOrdinal("InvoiceDate");
                        var invAmtOrdinal = reader.GetOrdinal("InvAmt");
                        var rxNumberOrdinal = reader.GetOrdinal("RxNumber");
                        var rxDateOrdinal = reader.GetOrdinal("RxDate");
                        var labelNameOrdinal = reader.GetOrdinal("LabelName");
                        var insuranceCarrierOrdinal = reader.GetOrdinal("InsuranceCarrier");
                        var pharmacyStateOrdinal = reader.GetOrdinal("PharmacyState");
                        var adjustorNameOrdinal = reader.GetOrdinal("AdjustorName");
                        var adjustorPhoneOrdinal = reader.GetOrdinal("AdjustorPhone");
                        
                        while (reader.Read())
                        {
                            var record = new UnpaidScriptResultDto
                            {
                                PrescriptionId = reader.GetInt32(prescriptionIdOrdinal),
                                ClaimId = reader.GetInt32(claimIdOrdinal),
                                PatientName = !reader.IsDBNull(patientNameOrdinal) ? reader.GetString(patientNameOrdinal) : string.Empty,
                                ClaimNumber = !reader.IsDBNull(claimNumberOrdinal) ? reader.GetString(claimNumberOrdinal) : string.Empty,
                                InvoiceNumber = !reader.IsDBNull(invoiceNumberOrdinal) ? reader.GetString(invoiceNumberOrdinal) : string.Empty,
                                InvoiceDate = !reader.IsDBNull(invDateOrdinal) ? reader.GetDateTime(invDateOrdinal) : new DateTime(),
                                InvAmt = !reader.IsDBNull(invAmtOrdinal) ? reader.GetDecimal(invAmtOrdinal) : 0.00m,
                                RxNumber = !reader.IsDBNull(rxNumberOrdinal) ? reader.GetString(rxNumberOrdinal) : string.Empty,
                                RxDate = !reader.IsDBNull(rxDateOrdinal) ? reader.GetDateTime(rxDateOrdinal) : new DateTime(),
                                LabelName = !reader.IsDBNull(labelNameOrdinal) ? reader.GetString(labelNameOrdinal) : string.Empty,
                                InsuranceCarrier = !reader.IsDBNull(insuranceCarrierOrdinal) ? reader.GetString(insuranceCarrierOrdinal) : string.Empty,
                                PharmacyState = !reader.IsDBNull(pharmacyStateOrdinal) ? reader.GetString(pharmacyStateOrdinal) : string.Empty,
                                AdjustorName = !reader.IsDBNull(adjustorNameOrdinal) ? reader.GetString(adjustorNameOrdinal) : string.Empty,
                                AdjustorPhone = !reader.IsDBNull(adjustorPhoneOrdinal) ? reader.GetString(adjustorPhoneOrdinal) : string.Empty
                            };
                            retValResults.Add(record);
                        }
                    });
                    retVal.UnpaidScriptResults = retValResults;
                    retVal.TotalRowCount = totalRowsParam.Value as int? ?? default(int);
                    return retVal;
                });
            });
        }
    }
}