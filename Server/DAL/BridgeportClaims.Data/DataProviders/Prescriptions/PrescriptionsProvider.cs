using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Data.Enums;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Data.StoredProcedureExecutors;
using BridgeportClaims.Entities.DomainModels;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.Prescriptions
{
    public class PrescriptionsProvider : IPrescriptionsProvider
    {
        private readonly IRepository<Prescription> _prescriptionRepository;
        private readonly IRepository<PrescriptionStatus> _prescriptionStatusRepository;
        private readonly IStoredProcedureExecutor _executor;

        public PrescriptionsProvider(IRepository<Prescription> prescriptionRepository, IRepository<PrescriptionStatus> prescriptionStatusRepository, IStoredProcedureExecutor executor)
        {
            _prescriptionRepository = prescriptionRepository;
            _prescriptionStatusRepository = prescriptionStatusRepository;
            _executor = executor;
        }

        public EntityOperation AddOrUpdatePrescriptionStatus(int prescriptionId, int prescriptionStatusId)
        {
            var prescription = _prescriptionRepository.Get(prescriptionId);
            if (null == prescription)
                throw new ArgumentNullException(nameof(prescription));
            var prescriptionStatus = _prescriptionStatusRepository.Get(prescriptionStatusId);
            var op = null == prescription.PrescriptionStatus ? EntityOperation.Add : EntityOperation.Update;
            prescription.PrescriptionStatus = prescriptionStatus ??
                                              throw new ArgumentNullException(nameof(prescriptionStatus));
            prescription.UpdatedOnUtc = DateTime.UtcNow;
            _prescriptionRepository.Update(prescription);
            return op;
        }

        public IList<UnpaidScriptsDto> GetUnpaidScripts(bool isDefaultSort, DateTime? startDate, DateTime? endDate,
            string sort, string sortDirection, int page, int pageSize)
            => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                return DisposableService.Using(() => new SqlCommand("[dbo].[uspGetUnpaidScripts]", conn), cmd =>
                    {
                        return DisposableService.Using(cmd.ExecuteReader, reader =>
                        {
                            var isDefaultSortParam = new SqlParameter
                            {
                                ParameterName = "@IsDefaultSort",
                                Value = isDefaultSort,
                                DbType = DbType.Boolean,
                                SqlDbType = SqlDbType.Bit,
                                Direction = ParameterDirection.Input
                            };
                            var startDateParam = new SqlParameter
                            {
                                ParameterName = "@StartDate",
                                DbType = DbType.Date,
                                Value = startDate,
                                SqlDbType = SqlDbType.Date,
                                Direction = ParameterDirection.Input
                            };
                            var endDateParam = new SqlParameter
                            {
                                ParameterName = "@EndDate",
                                Value = endDate,
                                DbType = DbType.Date,
                                Direction = ParameterDirection.Input,
                                SqlDbType = SqlDbType.Date
                            };
                            var sortParam = new SqlParameter
                            {
                                ParameterName = "@SortColumn",
                                DbType = DbType.String,
                                Value = sort,
                                Direction= ParameterDirection.Input,
                                SqlDbType = SqlDbType.NVarChar
                            };
                            var sortDirectionParam = new SqlParameter
                            {
                                ParameterName = "@SortDirection",
                                DbType = DbType.String,
                                Value = sortDirection,
                                SqlDbType = SqlDbType.VarChar,
                                Direction = ParameterDirection.Input
                            };
                            var pageParam = new SqlParameter
                            {
                                ParameterName = "@PageNumber",
                                DbType = DbType.Int32,
                                Value = page,
                                Direction = ParameterDirection.Input,
                                SqlDbType = SqlDbType.Int
                            };
                            var pageSizeParam = new SqlParameter
                            {
                                ParameterName = "@PageSize",
                                Value = pageSize,
                                DbType = DbType.Int32,
                                SqlDbType = SqlDbType.Int
                            };
                            //if (conn.State != ConnectionState.Open)
                                conn.Open();
                            cmd.Parameters.Add(isDefaultSortParam);
                            cmd.Parameters.Add(startDateParam);
                            cmd.Parameters.Add(endDateParam);
                            cmd.Parameters.Add(sortParam);
                            cmd.Parameters.Add(sortDirectionParam);
                            cmd.Parameters.Add(pageParam);
                            cmd.Parameters.Add(pageSizeParam);
                            IList <UnpaidScriptsDto> retVal = new List<UnpaidScriptsDto>();
                            var prescriptionIdOrdinal = reader.GetOrdinal("PrescriptionId");
                            var claimIdOrdinal = reader.GetOrdinal("ClaimId");
                            var ownerOrdinal = reader.GetOrdinal("Owner");
                            var patientNameOrdinal = reader.GetOrdinal("PatientName");
                            var claimNumberOrdinal = reader.GetOrdinal("ClaimNumber");
                            var invoiceNumberOrdinal = reader.GetOrdinal("InvoiceNumber");
                            var invDateOrdinal = reader.GetOrdinal("InvDate");
                            var invAmtOrdinal = reader.GetOrdinal("InvAmt");
                            var rxNumberOrdinal = reader.GetOrdinal("RxNumber");
                            var rxDateOrdinal = reader.GetOrdinal("RxDate");
                            var labelNameOrdinal = reader.GetOrdinal("LabelName");
                            var insuranceCarrierOrdinal = reader.GetOrdinal("InsuranceCarrier");
                            var pharmacyStateOrdinal = reader.GetOrdinal("PharmacyState");
                            var adjustorNameOrdinal = reader.GetOrdinal("AdjustorName");
                            var adjustorPhoneOrdinal = reader.GetOrdinal("AdjustorPhone");
                            try
                            {
                                while (reader.Read())
                                {
                                    var record = new UnpaidScriptsDto
                                    {
                                        PrescriptionId = reader.GetInt32(prescriptionIdOrdinal),
                                        ClaimId = reader.GetInt32(claimIdOrdinal),
                                        Owner = reader.IsDBNull(ownerOrdinal) ? null : reader.GetString(ownerOrdinal),
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
                                        AdjustorName = reader.GetString(adjustorNameOrdinal),
                                        AdjustorPhone = reader.GetString(adjustorPhoneOrdinal)

                                    };
                                    retVal.Add(record);
                                }
                                return retVal;
                            }
                            catch
                            {
                                throw new Exception("Cannot connect to the database.");
                            }
                            {
                                conn.Dispose();
                            }
                        });
                    });
            });
    }
}