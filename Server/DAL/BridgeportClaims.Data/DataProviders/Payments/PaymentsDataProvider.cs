using System;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using BridgeportClaims.Data.Dtos;
using System.Collections.Generic;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Common.Extensions;
using Dapper;
using cs = BridgeportClaims.Common.Config.ConfigService;
using s = BridgeportClaims.Common.Constants.StringConstants;

namespace BridgeportClaims.Data.DataProviders.Payments
{
    public class PaymentsDataProvider : IPaymentsDataProvider
    {
        public IEnumerable<ClaimsWithPrescriptionDetailsDto> GetClaimsWithPrescriptionDetails(IEnumerable<int> claimIds)
            => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                var delimitedClaimIds = string.Join(s.Comma, claimIds);
                const string sp = "[dbo].[uspGetClaimsWithPrescriptionDetails]";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<ClaimsWithPrescriptionDetailsDto>(sp, new {ClaimIDs = delimitedClaimIds},
                    commandType: CommandType.StoredProcedure)?.OrderByDescending(o => o.RxDate);
            });
        
        public void PrescriptionPostings(string checkNumber, bool hasSuspense, decimal? suspenseAmountRemaining,
                    string toSuspenseNoteText, int documentId, string userId, IList<PaymentPostingDto> paymentPostings)
            => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                DisposableService.Using(() => new SqlCommand("dbo.uspInsertPaymentPostings", conn), cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var checkNumberParam = cmd.CreateParameter();
                    checkNumberParam.Direction = ParameterDirection.Input;
                    checkNumberParam.ParameterName = "@CheckNumber";
                    checkNumberParam.DbType = DbType.AnsiString;
                    checkNumberParam.SqlDbType = SqlDbType.VarChar;
                    checkNumberParam.Size = 155;
                    checkNumberParam.Value = checkNumber;
                    cmd.Parameters.Add(checkNumberParam);
                    var hasSuspenseParam = cmd.CreateParameter();
                    hasSuspenseParam.Direction = ParameterDirection.Input;
                    hasSuspenseParam.Value = hasSuspense;
                    hasSuspenseParam.DbType = DbType.Boolean;
                    hasSuspenseParam.SqlDbType = SqlDbType.Bit;
                    hasSuspenseParam.ParameterName = "@HasSuspense";
                    cmd.Parameters.Add(hasSuspenseParam);
                    var suspenseAmountRemainingParam = cmd.CreateParameter();
                    suspenseAmountRemainingParam.Direction = ParameterDirection.Input;
                    suspenseAmountRemainingParam.ParameterName = "@SuspenseAmountRemaining";
                    suspenseAmountRemainingParam.DbType = DbType.Decimal;
                    suspenseAmountRemainingParam.SqlDbType = SqlDbType.Money;
                    suspenseAmountRemainingParam.Value = (object) suspenseAmountRemaining ?? DBNull.Value;
                    cmd.Parameters.Add(suspenseAmountRemainingParam);
                    var toSuspenseNoteTextParam = cmd.CreateParameter();
                    toSuspenseNoteTextParam.Value = (object) toSuspenseNoteText ?? DBNull.Value;
                    toSuspenseNoteTextParam.Direction = ParameterDirection.Input;
                    toSuspenseNoteTextParam.DbType = DbType.AnsiString;
                    toSuspenseNoteTextParam.Size = 255;
                    toSuspenseNoteTextParam.SqlDbType = SqlDbType.VarChar;
                    toSuspenseNoteTextParam.ParameterName = "@ToSuspenseNoteText";
                    cmd.Parameters.Add(toSuspenseNoteTextParam);
                    var documentIdParam = cmd.CreateParameter();
                    documentIdParam.Direction = ParameterDirection.Input;
                    documentIdParam.Value = (object) documentId ?? DBNull.Value;
                    documentIdParam.DbType = DbType.Int32;
                    documentIdParam.SqlDbType = SqlDbType.Int;
                    documentIdParam.ParameterName = "@DocumentID";
                    cmd.Parameters.Add(documentIdParam);
                    var userIdParam = cmd.CreateParameter();
                    userIdParam.Value = userId;
                    userIdParam.ParameterName = "@UserID";
                    userIdParam.DbType = DbType.String;
                    userIdParam.SqlDbType = SqlDbType.NVarChar;
                    userIdParam.Size = 128;
                    userIdParam.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(userIdParam);
                    var paymentPostingsParam = cmd.CreateParameter();
                    paymentPostingsParam.Direction = ParameterDirection.Input;
                    paymentPostingsParam.SqlDbType = SqlDbType.Structured;
                    paymentPostingsParam.Value = paymentPostings?.ToFixedDataTable();
                    paymentPostingsParam.ParameterName = "@PaymentPostings";
                    paymentPostingsParam.TypeName = "dbo.udtPaymentPosting";
                    cmd.Parameters.Add(paymentPostingsParam);
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    cmd.ExecuteNonQuery();
                });
            });

        public IEnumerable<PrescriptionPaymentsDto> GetPrescriptionPaymentsDtos(int claimId, string sortColumn,
            string direction, int pageNumber, int pageSize, string secondarySortColumn, string secondaryDirection) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[dbo].[uspGetPrescriptionPayments]";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                var ps = new DynamicParameters();
                ps.Add("@ClaimID", claimId, DbType.Int32);
                ps.Add("@SortColumn", sortColumn, DbType.AnsiString, size: 50);
                ps.Add("@SortDirection", direction, DbType.AnsiString, size: 5);
                ps.Add("@PageNumber", pageNumber, DbType.Int32);
                ps.Add("@PageSize", pageSize, DbType.Int32);
                ps.Add("@SecondarySortColumn", secondarySortColumn, DbType.AnsiString, size: 50);
                ps.Add("@SecondarySortDirection", secondaryDirection, DbType.AnsiString, size: 5);
                return conn.Query<PrescriptionPaymentsDto>(sp, ps, commandType: CommandType.StoredProcedure);
            });

        public IEnumerable<ClaimsWithPrescriptionCountsDto> GetClaimsWithPrescriptionCounts(string claimNumber,
            string firstName, string lastName, DateTime? rxDate, string invoiceNumber) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[dbo].[uspGetClaimsWithPrescriptionCounts]";
                conn.Open();
                return conn.Query<ClaimsWithPrescriptionCountsDto>(sp, new
                {
                    ClaimNumber = claimNumber,
                    FirstName = firstName,
                    LastName = lastName,
                    RxDate = rxDate,
                    InvoiceNumber = invoiceNumber
                }, commandType: CommandType.StoredProcedure);
            });
        
        public IEnumerable<byte> GetBytesFromDb(string fileName) => DisposableService.Using(()
            => new SqlConnection(cs.GetDbConnStr()), conn =>
        {
            return DisposableService.Using(() => new SqlCommand("[dbo].[uspGetFileBytesFromFileName]", conn),
                cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@FileName", SqlDbType.NVarChar, 255).Value = fileName;
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    return DisposableService.Using(cmd.ExecuteReader,
                        reader =>
                        {
                            if (reader.Read())
                                return (byte[]) reader["FileBytes"];
                            throw new Exception("Unable to read from the SqlDataReader");
                        });
                });
        });

        public void ImportDataTableIntoDb(DataTable dt) => DisposableService.Using(()
            => new SqlConnection(cs.GetDbConnStr()), conn =>
        {
            DisposableService.Using(() => new SqlCommand("dbo.uspImportPaymentFromDataTable", conn),
                cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var dataTableParam = new SqlParameter
                    {
                        Value = dt,
                        SqlDbType = SqlDbType.Structured,
                        ParameterName = "@Payment"
                    };
                    cmd.Parameters.Add(dataTableParam);
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    cmd.ExecuteNonQuery();
                });
        });

        private static DynamicParameters PrepareParameters(IEnumerable<int> prescriptionIds, string checkNumber,
            decimal checkAmount, decimal amountSelected, decimal amountToPost, int documentId)
        {
            var ps = new DynamicParameters();
            var dt = CreateDataTable(prescriptionIds);
            ps.Add("@PrescriptionIDs", dt.AsTableValuedParameter(s.UdtId));
            ps.Add("@DocumentID", documentId, DbType.Int32);
            ps.Add("@CheckNumber", checkNumber, DbType.AnsiString, size: 50);
            ps.Add("@CheckAmount", checkAmount, DbType.Decimal);
            ps.Add("@AmountSelected", amountSelected, DbType.Decimal);
            ps.Add("@AmountToPost", amountToPost, DbType.Decimal);
            return ps;
        }

        public PostPaymentReturnDto PostPayment(IEnumerable<int> prescriptionIds, string checkNumber,
            decimal checkAmount, decimal amountSelected, decimal amountToPost, int documentId)
        {
            var postPaymentReturnDto = new PostPaymentReturnDto();
            var ps = PrepareParameters(prescriptionIds, checkNumber, checkAmount, amountSelected, amountToPost, documentId);
            return DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[dbo].[uspPostPayment]";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                var multi = conn.QueryMultiple(sp, ps, commandTimeout: 180, commandType: CommandType.StoredProcedure);
                var postPaymentPrescriptionReturnDto = multi.Read<PostPaymentPrescriptionReturnDto>()?.ToList() ?? new List<PostPaymentPrescriptionReturnDto>();
                var postPaymentPrescriptionDocumentDto = multi.Read<PostPaymentPrescriptionDocumentDto>()?.SingleOrDefault() ?? new PostPaymentPrescriptionDocumentDto();
                postPaymentReturnDto.PostPaymentPrescriptionReturnDtos.AddRange(postPaymentPrescriptionReturnDto);
                postPaymentReturnDto.DocumentId = postPaymentPrescriptionDocumentDto.DocumentId;
                postPaymentReturnDto.FileName = postPaymentPrescriptionDocumentDto.FileName;
                postPaymentReturnDto.FileUrl = postPaymentPrescriptionDocumentDto.FileUrl;
                // TODO: Get rid of all of this shit.
                // postPaymentPrescriptionDocumentDto.AmountRemaining;
                // const NumberStyles style = NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint;
                // var culture = CultureInfo.CreateSpecificCulture("en-US");
                // decimal.TryParse(amtRemaining.ToString(CultureInfo.InvariantCulture), style, culture, out var d) ? d : default;
                postPaymentReturnDto.AmountRemaining = postPaymentPrescriptionDocumentDto.AmountRemaining;
                return postPaymentReturnDto;
            });
        }

        private static DataTable CreateDataTable(IEnumerable<int> ids)
        {
            var table = new DataTable();
            table.Columns.Add("ID", typeof(int));
            foreach (var id in ids)
            {
                table.Rows.Add(id);
            }
            return table;
        }
    }
}