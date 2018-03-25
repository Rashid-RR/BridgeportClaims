using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Data.SessionFactory.StoredProcedureExecutors;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.Diaries
{
    public class DiaryProvider : IDiaryProvider
    {
        private readonly IStoredProcedureExecutor _storedProcedureExecutor;

        public DiaryProvider(IStoredProcedureExecutor storedProcedureExecutor)
        {
            _storedProcedureExecutor = storedProcedureExecutor;
        }

        public IEnumerable<DiaryOwnerDto> GetDiaryOwners() =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                return DisposableService.Using(() => new SqlCommand("[dbo].[uspGetDiaryOwners]", conn), cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    IList<DiaryOwnerDto> retVal = new List<DiaryOwnerDto>();
                    DisposableService.Using(cmd.ExecuteReader, reader =>
                    {
                        var userIdOrdinal = reader.GetOrdinal("UserId");
                        var ownerOrdinal = reader.GetOrdinal("Owner");
                        if (conn.State != ConnectionState.Open)
                            conn.Open();
                        while (reader.Read())
                        {
                            var result = new DiaryOwnerDto
                            {
                                UserId = !reader.IsDBNull(userIdOrdinal) ? reader.GetString(userIdOrdinal) : string.Empty,
                                Owner = !reader.IsDBNull(ownerOrdinal) ? reader.GetString(ownerOrdinal) : string.Empty
                            };
                            retVal.Add(result);
                        }
                    });
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                    return retVal.AsEnumerable();
                });
            });

        public DiariesDto GetDiaries(bool isDefaultSort, DateTime? startDate, DateTime? endDate,
            string sortColumn, string sortDirection, int pageNumber, int pageSize, bool closed, string userId)
            => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                return DisposableService.Using(() => new SqlCommand("[dbo].[uspGetDiaries]", conn), cmd =>
                {
                    var isDefaultSortParam = cmd.CreateParameter();
                    isDefaultSortParam.Direction = ParameterDirection.Input;
                    isDefaultSortParam.DbType = DbType.Boolean;
                    isDefaultSortParam.SqlDbType = SqlDbType.Bit;
                    isDefaultSortParam.ParameterName = "@IsDefaultSort";
                    isDefaultSortParam.Value = isDefaultSort;
                    cmd.Parameters.Add(isDefaultSortParam);
                    var startDateParam = cmd.CreateParameter();
                    startDateParam.Direction = ParameterDirection.Input;
                    startDateParam.DbType = DbType.Date;
                    startDateParam.SqlDbType = SqlDbType.Date;
                    startDateParam.ParameterName = "@StartDate";
                    startDateParam.Value = startDate ?? (object) DBNull.Value;
                    cmd.Parameters.Add(startDateParam);
                    var endDateParam = cmd.CreateParameter();
                    endDateParam.Value = endDate ?? (object) DBNull.Value;
                    endDateParam.ParameterName = "@EndDate";
                    endDateParam.DbType = DbType.Date;
                    endDateParam.SqlDbType = SqlDbType.Date;
                    endDateParam.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(endDateParam);
                    var sortParam = cmd.CreateParameter();
                    sortParam.ParameterName = "@SortColumn";
                    sortParam.DbType = DbType.String;
                    sortParam.Value = sortColumn;
                    sortParam.Direction = ParameterDirection.Input;
                    sortParam.SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.Add(sortParam);
                    var sortDirectionParam = cmd.CreateParameter();
                    sortDirectionParam.ParameterName = "@SortDirection";
                    sortDirectionParam.DbType = DbType.String;
                    sortDirectionParam.Value = sortDirection;
                    sortDirectionParam.SqlDbType = SqlDbType.VarChar;
                    sortDirectionParam.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(sortDirectionParam);
                    var pageParam = cmd.CreateParameter();
                    pageParam.ParameterName = "@PageNumber";
                    pageParam.DbType = DbType.Int32;
                    pageParam.Value = pageNumber;
                    pageParam.Direction = ParameterDirection.Input;
                    pageParam.SqlDbType = SqlDbType.Int;
                    cmd.Parameters.Add(pageParam);
                    var pageSizeParam = cmd.CreateParameter();
                    pageSizeParam.ParameterName = "@PageSize";
                    pageSizeParam.Value = pageSize;
                    pageSizeParam.DbType = DbType.Int32;
                    pageSizeParam.SqlDbType = SqlDbType.Int;
                    pageSizeParam.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(pageSizeParam);
                    var closedParam = cmd.CreateParameter();
                    closedParam.Direction = ParameterDirection.Input;
                    closedParam.Value = closed;
                    closedParam.ParameterName = "@Closed";
                    closedParam.DbType = DbType.Boolean;
                    closedParam.SqlDbType = SqlDbType.Bit;
                    cmd.Parameters.Add(closedParam);
                    var userIdParam = cmd.CreateParameter();
                    userIdParam.Direction = ParameterDirection.Input;
                    userIdParam.Value = userId ?? (object) DBNull.Value;
                    userIdParam.ParameterName = "@UserID";
                    userIdParam.DbType = DbType.String;
                    userIdParam.SqlDbType = SqlDbType.NVarChar;
                    userIdParam.Size = 128;
                    cmd.Parameters.Add(userIdParam);
                    var totalRowsParam = cmd.CreateParameter();
                    totalRowsParam.ParameterName = "@TotalRows";
                    totalRowsParam.DbType = DbType.Int32;
                    totalRowsParam.SqlDbType = SqlDbType.Int;
                    totalRowsParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(totalRowsParam);
                    cmd.CommandType = CommandType.StoredProcedure;
                    var retVal = new DiariesDto();
                    IList<DiaryResultDto> retValResults = new List<DiaryResultDto>();
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    DisposableService.Using(cmd.ExecuteReader, reader =>
                    {
                        var diaryIdOrdinal = reader.GetOrdinal("DiaryId");
                        var claimIdOrdinal = reader.GetOrdinal("ClaimId");
                        var prescriptionNoteIdOrdinal = reader.GetOrdinal("PrescriptionNoteId");
                        var ownerOrdinal = reader.GetOrdinal("Owner");
                        var createdOrdinal = reader.GetOrdinal("Created");
                        var followUpDateOrdinal = reader.GetOrdinal("FollowUpDate");
                        var patientNameOrdinal = reader.GetOrdinal("PatientName");
                        var claimNumberOrdinal = reader.GetOrdinal("ClaimNumber");
                        var typeOrdinal = reader.GetOrdinal("Type");
                        var rxNumberOrdinal = reader.GetOrdinal("RxNumber");
                        var rxDateOrdinal = reader.GetOrdinal("RxDate");
                        var insuranceCarrierOrdinal = reader.GetOrdinal("InsuranceCarrier");
                        var diaryNoteOrdinal = reader.GetOrdinal("DiaryNote");
                        while (reader.Read())
                        {
                            var record = new DiaryResultDto
                            {
                                DiaryId = reader.GetInt32(diaryIdOrdinal),
                                ClaimId = reader.GetInt32(claimIdOrdinal),
                                PrescriptionNoteId = reader.GetInt32(prescriptionNoteIdOrdinal),
                                Owner = !reader.IsDBNull(ownerOrdinal) ? reader.GetString(ownerOrdinal) : string.Empty,
                                Created = reader.GetDateTime(createdOrdinal),
                                FollowUpDate = reader.GetDateTime(followUpDateOrdinal),
                                PatientName = !reader.IsDBNull(patientNameOrdinal) ? reader.GetString(patientNameOrdinal) : string.Empty,
                                ClaimNumber = !reader.IsDBNull(claimNumberOrdinal) ? reader.GetString(claimNumberOrdinal) : string.Empty,
                                Type = !reader.IsDBNull(typeOrdinal) ? reader.GetString(typeOrdinal) : string.Empty,
                                RxNumber = !reader.IsDBNull(rxNumberOrdinal) ? reader.GetString(rxNumberOrdinal) : string.Empty,
                                RxDate = reader.GetDateTime(rxDateOrdinal),
                                InsuranceCarrier = !reader.IsDBNull(insuranceCarrierOrdinal) ? reader.GetString(insuranceCarrierOrdinal) : string.Empty,
                                DiaryNote = !reader.IsDBNull(diaryNoteOrdinal) ? reader.GetString(diaryNoteOrdinal) : string.Empty
                            };
                            retValResults.Add(record);
                        }
                    });
                    retVal.DiaryResults = retValResults;
                    retVal.TotalRowCount = totalRowsParam.Value as int? ?? default(int);
                    return retVal;
                });
            });
            /*var startDateParam = new SqlParameter
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
            var closedParam = new SqlParameter
            {
                ParameterName = "Closed",
                Value = closed,
                DbType = DbType.Boolean
            };
            var retVal = _storedProcedureExecutor.ExecuteMultiResultStoredProcedure<DiariesDto>(
                "EXECUTE [dbo].[uspGetDiaries] @IsDefaultSort = :IsDefaultSort, @StartDate = :StartDate," +
                "@EndDate = :EndDate, @SortColumn = :SortColumn, @SortDirection = :SortDirection, @PageNumber = :PageNumber, @PageSize = :PageSize, @Closed = :Closed",
                new List<SqlParameter> { isDefaultSortParam, startDateParam, endDateParam, sortColumnParam, sortDirectionParam, pageNumberParam, pageSizeParam, closedParam })?.ToList();
            return retVal;
        }*/

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