using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.ClaimImages;
using BridgeportClaims.Data.DataProviders.Episodes;
using BridgeportClaims.Data.DataProviders.Payments;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Data.Enums;
using Dapper;
using NLog;
using cs = BridgeportClaims.Common.Config.ConfigService;
using ic = BridgeportClaims.Common.Constants.IntegerConstants;

namespace BridgeportClaims.Data.DataProviders.Claims
{
    public class ClaimsDataProvider : IClaimsDataProvider
    {
        private const string Query =  @"DECLARE @ClaimID INTEGER = {0};
                                        SELECT          [p].[FirstName]
                                                      , [p].[LastName]
                                                      , [p].[DateOfBirth]
                                        FROM            [dbo].[Patient] AS [p]
                                            INNER JOIN  [dbo].[Claim]   AS [c] ON [c].[PatientID] = [p].[PatientID]
                                        WHERE           [c].[ClaimID] = @ClaimID";
        private readonly Lazy<IEpisodesDataProvider> _episodesDataProvider;
        private readonly Lazy<IPaymentsDataProvider> _paymentsDataProvider;
        private readonly Lazy<IClaimImageProvider> _claimImageProvider;
        private static readonly Lazy<Logger> Logger = new Lazy<Logger>(LogManager.GetCurrentClassLogger);

        public ClaimsDataProvider(Lazy<IPaymentsDataProvider> paymentsDataProvider,
            Lazy<IClaimImageProvider> claimImageProvider,
            Lazy<IEpisodesDataProvider> episodesDataProvider)
        {
            _paymentsDataProvider = paymentsDataProvider;
            _claimImageProvider = claimImageProvider;
            _episodesDataProvider = episodesDataProvider;
        }

        public IList<GetClaimsSearchResults> GetClaimsData(string claimNumber, string firstName, string lastName,
            string rxNumber, string invoiceNumber) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[claims].[uspGetClaimsSearchResults]";
                var ps = new DynamicParameters();
                ps.Add("@ClaimNumber", claimNumber, DbType.AnsiString);
                ps.Add("@FirstName", firstName, DbType.AnsiString);
                ps.Add("@LastName", lastName, DbType.AnsiString);
                ps.Add("@RxNumber", rxNumber, DbType.AnsiString);
                ps.Add("@InvoiceNumber", invoiceNumber, DbType.String);
                conn.Open();
                var results = conn.Query<GetClaimsSearchResults>(sp, ps, commandType: CommandType.StoredProcedure);
                return results?.ToList();
            });

        public IList<PrescriptionDto> GetPrescriptionDataByClaim(int claimId, string sort, string direction, int page,
            int pageSize) => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
        {
            const string sp = "[claims].[uspGetPrescriptionDataForClaim]";
            var ps = new DynamicParameters();
            ps.Add("@ClaimID", claimId, DbType.Int32);
            ps.Add("@SortColumn", sort, DbType.AnsiString, ParameterDirection.Input, 50);
            ps.Add("@SortDirection", direction, DbType.AnsiString, ParameterDirection.Input, 5);
            ps.Add("@PageNumber", page, DbType.Int32);
            ps.Add("@PageSize", pageSize, DbType.Int32);
            conn.Open();
            var results = conn.Query<PrescriptionDto>(sp, ps, commandType: CommandType.StoredProcedure);
            return results?.ToList();
        });

        public EntityOperation AddOrUpdateFlex2(int claimId, int claimFlex2Id, string modifiedByUserId) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[claims].[uspClaimFlex2Update]";
                var ps = new DynamicParameters();
                ps.Add("@ClaimID", claimId, DbType.Int32);
                ps.Add("@ClaimFlex2ID", claimFlex2Id, DbType.Int32);
                ps.Add("@ModifiedByUserID", modifiedByUserId, DbType.String);
                ps.Add("@Operation", dbType: DbType.AnsiString, direction: ParameterDirection.Output, size: 10);
                conn.Open();
                conn.Execute(sp, ps, commandType: CommandType.StoredProcedure);
                var operation = ps.Get<string>("@Operation");
                if (operation.IsNullOrWhiteSpace())
                    throw new ArgumentNullException(nameof(operation));
                return operation.ToLower() == "add" ? EntityOperation.Add : EntityOperation.Update;
            });

        public IEnumerable<EpisodeBladeDto> GetEpisodesBlade(int claimId, string sortColumn, string sortDirection) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[dbo].[uspGetEpisodesBlade]";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                var ps = new DynamicParameters();
                ps.Add("@ClaimID", claimId, DbType.Int32);
                ps.Add("@SortColumn", sortColumn, DbType.AnsiString, size: 50);
                ps.Add("@SortDirection", sortDirection, DbType.AnsiString, size: 5);
                return conn.Query<EpisodeBladeDto>(sp, ps, commandType: CommandType.StoredProcedure);
            });

        public ClaimDto GetClaimsDataByClaimId(int claimId, string userId) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                try
                {
                    var hasEnoughScripts = false;
                    decimal totalPayableAmount = 0;
                    decimal totalAmountPaid = 0;

                    const string sp = "[claims].[uspGetClaims]";
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    var multi = conn.QueryMultiple(sp, new {ClaimID = claimId},
                        commandType: CommandType.StoredProcedure);
                    var claimDto = multi.Read<ClaimDto>()?.SingleOrDefault();
                    if (null == claimDto)
                        return null;
                    var claimFlex2Dto = multi.Read<ClaimFlex2Dto>()?.OrderBy(x => x.Flex2).ToList();
                    if (null != claimFlex2Dto)
                        claimDto.ClaimFlex2s = claimFlex2Dto;
                    var claimNoteDto = multi.Read<ClaimNoteDto>()?.ToList();
                    if (null != claimNoteDto)
                        claimDto.ClaimNotes = claimNoteDto;
                    var episodes = GetEpisodesBlade(claimId, "Created", "DESC")?.ToList();
                    if (null != episodes)
                        claimDto.Episodes = episodes;
                    const string spObj = "[claims].[uspGetClaimObjects]";
                    var multiObj = conn.QueryMultiple(spObj, new {ClaimID = claimId},
                        commandType: CommandType.StoredProcedure);
                    // Document Types
                    var documentTypes = multiObj.Read<DocumentTypeDto>()?.OrderBy(x => x.TypeName).ToList();
                    if (null != documentTypes)
                        claimDto.DocumentTypes = documentTypes;
                    var acctPayableDtos = multiObj.Read<AcctPayableDto>()?.ToList();
                    if (null != acctPayableDtos)
                        claimDto.AcctPayables = acctPayableDtos;
                    // Prescription Statuses
                    var prescriptionStatuses =
                        multiObj.Read<PrescriptionStatusDto>()?.OrderBy(x => x.StatusName).ToList();
                    if (null != prescriptionStatuses)
                        claimDto.PrescriptionStatuses = prescriptionStatuses;
                    // Genders
                    var genders = multiObj.Read<GenderDto>()?.ToList();
                    if (null != genders)
                        claimDto.Genders = genders;
                    // U.S. States
                    var states = multiObj.Read<UsStateDto>()?.OrderBy(x => x.StateName).ToList();
                    if (null != states)
                        claimDto.States = states;
                    // Payments
                    var payments = _paymentsDataProvider.Value.GetPrescriptionPaymentsDtos(
                        claimId, "RxDate", "DESC", 1, ic.MaxRowCountForBladeInApp, "RxNumber", "ASC")?.ToList();
                    if (null != payments)
                    {
                        totalPayableAmount = payments.Sum(s => s.PayableAmount);
                        claimDto.Payments = payments;
                    }
                    // Outstanding
                    var outstanding = GetOutstanding(claimId, 1, ic.MaxRowCountForBladeInApp, "RxDate", "ASC");
                    if (null != outstanding)
                    {
                        claimDto.Outstanding = outstanding;
                    }
                    // Episodes Types
                    var episodeTypes = _episodesDataProvider.Value?.GetEpisodeTypes();
                    if (null != episodeTypes)
                        claimDto.EpisodeTypes = episodeTypes.OrderBy(x => x.SortOrder).ToList();
                    // Claim Prescriptions
                    var scripts = GetPrescriptionDataByClaim(claimId, "RxDate", "DESC", 1, ic.MaxRowCountForBladeInApp)?.ToList();
                    if (null != scripts && scripts.Any())
                    {
                        totalAmountPaid = scripts?.Sum(s => s.AmountPaid) ?? 0;
                        if ((scripts?.Count ?? 0) >= ic.NumberOfScriptsNeededToBeVip)
                        {
                            hasEnoughScripts = true;
                        }
                        claimDto.Prescriptions = scripts ?? new List<PrescriptionDto>();
                    }
                    // VIP Logic.
                    // Collected Margin > $5,000.  Collected Margin is defined as 
                    // sum(PrescriptionPayment.AmountPaid - Prescription.PayableAmount)
                    if (hasEnoughScripts) // First check passed.
                    {
                        var collectedMargin = totalAmountPaid - totalPayableAmount;
                        if (collectedMargin > ic.CollectedMarginMasi)
                        {
                            claimDto.isVip = true;
                        }
                    }

                    // Prescription Notes
                    var prescriptionNotesDtos = GetPrescriptionNotes(claimId)?.ToList();
                    var scriptNotesDtos = prescriptionNotesDtos?.GroupBy(r => new
                    {
                        r.ClaimId,
                        r.PrescriptionNoteId,
                        r.Type,
                        r.EnteredBy,
                        r.Note,
                        r.NoteUpdatedOn,
                        r.HasDiaryEntry,
                        r.DiaryId
                    }).Select(gcs => new ScriptNoteDto
                    {
                        ClaimId = gcs.Key.ClaimId,
                        Scripts = gcs.Select(x => new ScriptDto {RxNumber = x.RxNumber, RxDate = x.RxDate})
                            .OrderByDescending(x => x.RxDate)
                            .ThenBy(x => x.RxNumber)
                            .ToList(),
                        EnteredBy = gcs.Key.EnteredBy,
                        HasDiaryEntry = gcs.Key.HasDiaryEntry,
                        DiaryId = gcs.Key.DiaryId,
                        Note = gcs.Key.Note,
                        NoteUpdatedOn = gcs.Key.NoteUpdatedOn,
                        PrescriptionNoteId = gcs.Key.PrescriptionNoteId,
                        Type = gcs.Key.Type
                    }).ToList() ?? new List<ScriptNoteDto>();
                    claimDto.PrescriptionNotes = scriptNotesDtos;
                    var imageResults = _claimImageProvider.Value.GetClaimImages(
                        claimId, "Created", "DESC", 1, ic.MaxRowCountForBladeInApp);
                    if (null != imageResults)
                        claimDto.Images = imageResults.ClaimImages;
                    return claimDto;
                }
                catch (Exception ex)
                {
                    Logger.Value.Error(ex);
                    throw;
                }
            });

        private static IEnumerable<PrescriptionNotesDto> GetPrescriptionNotes(int claimId) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[claims].[uspGetPrescriptionNotes]";
                conn.Open();
                return conn.Query<PrescriptionNotesDto>(sp, new {ClaimID = claimId}, commandType: CommandType.StoredProcedure);
            });

        public BillingStatementDto GetBillingStatementDto(int claimId) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                var query = string.Format(Query, claimId);
                return DisposableService.Using(() => new SqlCommand(query, conn), cmd =>
                {
                    cmd.CommandType = CommandType.Text;
                    BillingStatementDto retVal = null;
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    DisposableService.Using(cmd.ExecuteReader, reader =>
                    {
                        var firstNameOrdinal = reader.GetOrdinal("FirstName");
                        var lastNameOrdinal = reader.GetOrdinal("LastName");
                        var dateOfBirthOrdinal = reader.GetOrdinal("DateOfBirth");
                        while (reader.Read())
                        {
                            retVal = new BillingStatementDto
                            {
                                FirstName = !reader.IsDBNull(firstNameOrdinal) ? reader.GetString(firstNameOrdinal) : string.Empty,
                                LastName = !reader.IsDBNull(lastNameOrdinal) ? reader.GetString(lastNameOrdinal) : string.Empty,
                                DateOfBirth = !reader.IsDBNull(dateOfBirthOrdinal) ? reader.GetDateTime(dateOfBirthOrdinal) : (DateTime?) null
                            };
                        }
                    });
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                    return retVal;
                });
            });

        public string UpdateIsMaxBalance(int claimId, bool isMaxBalance, string modifiedByUserId)
            => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[dbo].[uspClaimUpdateMaxBalance]";
                var ps = new DynamicParameters();
                ps.Add("@ClaimID", claimId, DbType.Int32);
                ps.Add("@IsMaxBalance", isMaxBalance, DbType.Boolean);
                ps.Add("@ModifiedByUserID", modifiedByUserId, DbType.String, size: 128);
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                conn.Execute(sp, ps, commandTimeout: 1800, commandType: CommandType.StoredProcedure);
                var msg = $"Claim Id {claimId} has been {(isMaxBalance ? string.Empty : "un")}blocked from a Max Balance.";
                return msg;
            });

        public OutstandingDto GetOutstanding(int claimId, int pageNumber,
            int pageSize, string sortColumn, string sortDirection) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[claims].[uspGetOutstandingBlade]";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                var ps = new DynamicParameters();
                ps.Add("@ClaimID", claimId, DbType.Int32);
                ps.Add("@PageNumber", pageNumber, DbType.Int32);
                ps.Add("@PageSize", pageSize, DbType.Int32);
                ps.Add("@SortColumn", sortColumn, DbType.AnsiString, size: 50);
                ps.Add("@SortDirection", sortDirection, DbType.AnsiString, size: 5);
                var multi = conn.QueryMultiple(sp, ps, commandType: CommandType.StoredProcedure);
                var results = multi.Read<OutstandingDtoResult>();
                var totals = multi.Read<OutstandingDtoTotalsResult>()?.SingleOrDefault() ??
                             throw new Exception($"Totals cannot be read from the stored proc: {sp}.");
                var totalRows = totals.TotalRows;
                var totalOutstanding = totals.TotalOutstanding;
                var retVal = new OutstandingDto
                {
                    Results = results, TotalOutstanding = totalOutstanding, TotalRows = totalRows
                };
                return retVal;
            });
    }
}