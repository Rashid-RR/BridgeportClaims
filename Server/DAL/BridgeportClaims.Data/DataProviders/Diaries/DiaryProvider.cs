using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Data.StoredProcedureExecutors;

namespace BridgeportClaims.Data.DataProviders.Diaries
{
    public class DiaryProvider : IDiaryProvider
    {
        private readonly IStoredProcedureExecutor _storedProcedureExecutor;

        public DiaryProvider(IStoredProcedureExecutor storedProcedureExecutor)
        {
            _storedProcedureExecutor = storedProcedureExecutor;
        }

        public IList<DiariesDto> GetDiaries(bool isDefaultSort, DateTime? startDate, DateTime? endDate,
            string sortColumn, string sortDirection, int pageNumber, int pageSize)
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
                DbType = DbType.Date,
                Value = endDate
            };
            var sortColumnParam = new SqlParameter
            {
                ParameterName = "SortColumn",
                Value = sortColumn,
                DbType = DbType.String
            };
            var sortDirectionParam = new SqlParameter
            {
                ParameterName = "SortDirection",
                Value = sortDirection,
                DbType = DbType.String
            };
            var pageNumberParam = new SqlParameter
            {
                ParameterName = "PageNumber",
                DbType = DbType.Int32,
                Value = pageNumber
            };
            var pageSizeParam = new SqlParameter
            {
                ParameterName = "PageSize",
                DbType = DbType.Int32,
                Value = pageSize
            };
            var retVal = _storedProcedureExecutor.ExecuteMultiResultStoredProcedure<DiariesDto>(
                "EXECUTE dbo.uspGetDiaries @IsDefaultSort = :IsDefaultSort, @StartDate = :StartDate," +
                "@EndDate = :EndDate, @SortColumn = :SortColumn, @SortDirection = :SortDirection, @PageNumber = :PageNumber, @PageSize = :PageSize",
                new List<SqlParameter> { isDefaultSortParam, startDateParam, endDateParam, sortColumnParam, sortDirectionParam, pageNumberParam, pageSizeParam })?.ToList();
            return retVal;
        }

        public void RemoveDiary(int prescriptionNoteId)
        {
            _storedProcedureExecutor.ExecuteNoResultStoredProcedure(
                "EXECUTE dbo.uspUpdateDiary @PrescriptionNoteID = :PrescriptionNoteID",
                new List<SqlParameter>
                {
                    new SqlParameter
                    {
                        ParameterName = "PrescriptionNoteID",
                        DbType = DbType.Int32,
                        Value = prescriptionNoteId
                    }
                });
        }
    }
}