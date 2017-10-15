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

        public IList<UnpaidScriptsDto> GetUnpaidScripts(bool isDefaultSort, DateTime? startDate, DateTime? endDate,
            string sort, string sortDirection, int page, int pageSize)
        {
            return DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                return DisposableService.Using(() => new SqlCommand("[dbo].[uspGetUnpaidScripts]", conn), cmd =>
                {
                    var isDefaultSortParam = cmd.CreateParameter();
                    isDefaultSortParam.ParameterName = "IsDefaultSort";
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
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(isDefaultSortParam);
                    cmd.Parameters.Add(startDateParam);
                    cmd.Parameters.Add(endDateParam);
                    cmd.Parameters.Add(sortParam);
                    cmd.Parameters.Add(sortDirectionParam);
                    cmd.Parameters.Add(pageParam);
                    cmd.Parameters.Add(pageSizeParam);
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    return DisposableService.Using(cmd.ExecuteReader, reader =>
                    {
                        IList<UnpaidScriptsDto> retVal = new List<UnpaidScriptsDto>();
                        var prescriptionIdOrdinal = reader.GetOrdinal("PrescriptionId");
                        var claimIdOrdinal = reader.GetOrdinal("ClaimId");
                        var ownerOrdinal = reader.GetOrdinal("Owner");
                        var createdOrdinal = reader.GetOrdinal("Created");
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
                            var record = new UnpaidScriptsDto
                            {
                                PrescriptionId = reader.GetInt32(prescriptionIdOrdinal),
                                ClaimId = reader.GetInt32(claimIdOrdinal),
                                Owner = !reader.IsDBNull(ownerOrdinal) ? reader.GetString(ownerOrdinal) : string.Empty,
                                Created = !reader.IsDBNull(createdOrdinal)
                                    ? reader.GetDateTime(createdOrdinal)
                                    : DateTime.UtcNow,
                                PatientName = reader.GetString(patientNameOrdinal),
                                ClaimNumber = reader.GetString(claimNumberOrdinal),
                                InvoiceNumber = reader.GetString(invoiceNumberOrdinal),
                                InvoiceDate = reader.GetDateTime(invDateOrdinal),
                                InvAmt = reader.GetDecimal(invAmtOrdinal),
                                RxNumber = reader.GetString(rxNumberOrdinal),
                                RxDate = reader.GetDateTime(rxDateOrdinal),
                                LabelName = reader.GetString(labelNameOrdinal),
                                InsuranceCarrier = reader.GetString(insuranceCarrierOrdinal),
                                PharmacyState = reader.GetString(pharmacyStateOrdinal),
                                AdjustorName = !reader.IsDBNull(adjustorNameOrdinal)
                                            ? reader.GetString(adjustorNameOrdinal)
                                            : string.Empty,
                                AdjustorPhone = !reader.IsDBNull(adjustorPhoneOrdinal)
                            retVal.Add(record);
                            ///conn.Close();
                        }
                        return retVal;
                    });
                });
            });
        }
    }
}