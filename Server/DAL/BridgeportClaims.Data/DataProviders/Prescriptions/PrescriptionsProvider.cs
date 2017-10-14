using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Data.Enums;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Data.StoredProcedureExecutors;
using BridgeportClaims.Entities.DomainModels;

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
        {
            var isDefaultSortParam = new SqlParameter
            {
                ParameterName = "IsDefaultSort",
                Value = isDefaultSort,
                DbType = DbType.Boolean
            };
            var startDateParam = new SqlParameter
             {
                 ParameterName = "StartDate",
                 DbType = DbType.Date,
                 Value = startDate
             };
             var endDateParam = new SqlParameter
             {
                 ParameterName = "EndDate",
                 Value = endDate,
                 DbType = DbType.Date
             };
             var sortParam = new SqlParameter
             {
                 ParameterName = "SortColumn",
                 DbType = DbType.String,
                 Value = sort
             };
             var sortDirectionParam = new SqlParameter
             {
                 ParameterName = "SortDirection",
                 DbType = DbType.String,
                 Value = sortDirection
             };
             var pageParam = new SqlParameter
             {
                 ParameterName = "PageNumber",
                 DbType = DbType.Int32,
                 Value = page
             };
             var pageSizeParam = new SqlParameter
             {
                 ParameterName = "PageSize",
                 Value = pageSize,
                 DbType = DbType.Int32
             };
            var results = _executor.ExecuteMultiResultStoredProcedure<UnpaidScriptsDto>(
                "EXECUTE [dbo].[uspGetUnpaidScripts] @IsDefaultSort = :IsDefaultSort, @StartDate = :StartDate, @EndDate = :EndDate, " +
                "@SortColumn = :SortColumn, @SortDirection = :SortDirection, @PageNumber = :PageNumber, @PageSize = :PageSize",
                new List<SqlParameter> {isDefaultSortParam, startDateParam, endDateParam, sortParam, sortDirectionParam, pageParam, pageSizeParam})?.ToList();
            return results;
        }
    }
}