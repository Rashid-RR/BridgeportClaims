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

		public IList<EpisodeBladeDto> GetEpisodesBlade(int claimId, string sortColumn, string sortDirection, string userId) =>
			DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
			{
				return DisposableService.Using(() => new SqlCommand("[dbo].[uspGetEpisodesBlade]", conn), cmd =>
				{
					cmd.CommandType = CommandType.StoredProcedure;
					IList<EpisodeBladeDto> retVal = new List<EpisodeBladeDto>();
					var claimIdParam = cmd.CreateParameter();
					claimIdParam.DbType = DbType.Int32;
					claimIdParam.SqlDbType = SqlDbType.Int;
					claimIdParam.Value = claimId;
					claimIdParam.ParameterName = "@ClaimID";
					claimIdParam.Direction = ParameterDirection.Input;
					cmd.Parameters.Add(claimIdParam);
					var sortColumnParam = cmd.CreateParameter();
					sortColumnParam.DbType = DbType.AnsiString;
					sortColumnParam.SqlDbType = SqlDbType.VarChar;
					sortColumnParam.Size = 50;
					sortColumnParam.ParameterName = "@SortColumn";
					sortColumnParam.Direction = ParameterDirection.Input;
					sortColumnParam.Value = sortColumn ?? (object) DBNull.Value;
					cmd.Parameters.Add(sortColumnParam);
					var sortDirectionParam = cmd.CreateParameter();
					sortDirectionParam.Value = sortDirection ?? (object) DBNull.Value;
					sortDirectionParam.DbType = DbType.AnsiString;
					sortDirectionParam.SqlDbType = SqlDbType.VarChar;
					sortDirectionParam.Size = 5;
					sortDirectionParam.ParameterName = "@SortDirection";
					sortDirectionParam.Direction = ParameterDirection.Input;
					cmd.Parameters.Add(sortDirectionParam);
					if (conn.State != ConnectionState.Open)
						conn.Open();
					DisposableService.Using(cmd.ExecuteReader, reader =>
					{
						var idOrdinal = reader.GetOrdinal("Id");
						var createdOrdinal = reader.GetOrdinal("Created");
						var ownerOrdinal = reader.GetOrdinal("Owner");
						var typeOrdinal = reader.GetOrdinal("Type");
						var roleOrdinal = reader.GetOrdinal("Role");
						var pharmacyOrdinal = reader.GetOrdinal("Pharmacy");
						var rxNumberOrdinal = reader.GetOrdinal("RxNumber");
						var resolvedOrdinal = reader.GetOrdinal("Resolved");
						var noteCountOrdinal = reader.GetOrdinal("NoteCount");
						while (reader.Read())
						{
							var result = new EpisodeBladeDto
							{
								Id = !reader.IsDBNull(idOrdinal) ? reader.GetInt32(idOrdinal) : default,
								Created = !reader.IsDBNull(createdOrdinal) ? reader.GetDateTime(createdOrdinal) : (DateTime?) null,
								Owner = !reader.IsDBNull(ownerOrdinal) ? reader.GetString(ownerOrdinal) : string.Empty,
								Type = !reader.IsDBNull(typeOrdinal) ? reader.GetString(typeOrdinal) : string.Empty,
								Role = !reader.IsDBNull(roleOrdinal) ? reader.GetString(roleOrdinal) : string.Empty,
								Pharmacy = !reader.IsDBNull(pharmacyOrdinal) ? reader.GetString(pharmacyOrdinal) : string.Empty,
								RxNumber = !reader.IsDBNull(rxNumberOrdinal) ? reader.GetString(rxNumberOrdinal) : string.Empty,
								Resolved = !reader.IsDBNull(resolvedOrdinal) && reader.GetBoolean(resolvedOrdinal),
								NoteCount = !reader.IsDBNull(noteCountOrdinal) ? reader.GetInt32(noteCountOrdinal) : default
							};
							retVal.Add(result);
						}
					});
					if (conn.State != ConnectionState.Closed)
						conn.Close();
					return retVal;
				});
			});

		public ClaimDto GetClaimsDataByClaimId(int claimId, string userId) =>
			DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
			{
			    try
			    {
			        const string sp = "[claims].[uspGetClaims]";
			        conn.Open();
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
			        var episodes = GetEpisodesBlade(claimId, "Created", "DESC", userId);
			        if (null != episodes)
			            claimDto.Episodes = episodes;
			        const string spObj = "[claims].[uspGetClaimObjects]";
			        var multiObj = conn.QueryMultiple(spObj, new {ClaimID = claimId},
			            commandType: CommandType.StoredProcedure);
			        var documentTypes = multiObj.Read<DocumentTypeDto>()?.OrderBy(x => x.TypeName).ToList();
			        if (null != documentTypes)
			            claimDto.DocumentTypes = documentTypes;
			        var acctPayableDtos = multiObj.Read<AcctPayableDto>()?.ToList();
			        if (null != acctPayableDtos)
			            claimDto.AcctPayables = acctPayableDtos;
			        // U.S. States
			        var states = multiObj.Read<UsStateDto>()?.OrderBy(x => x.StateName).ToList();
			        if (null != states)
			            claimDto.States = states;
			        // Prescription Statuses
			        var prescriptionStatuses =
			            multiObj.Read<PrescriptionStatusDto>()?.OrderBy(x => x.StatusName)?.ToList();
			        if (null != prescriptionStatuses)
			            claimDto.PrescriptionStatuses = prescriptionStatuses;
			        // Payments
			        var payments = _paymentsDataProvider.Value.GetPrescriptionPaymentsDtos(
			            claimId, "RxDate", "DESC", 1, ic.MaxRowCountForBladeInApp, "RxNumber", "ASC");
			        if (null != payments)
			            claimDto.Payments = payments;
			        // Genders
			        var genders = multiObj.Read<GenderDto>()?.ToList();
			        if (null != genders)
			            claimDto.Genders = genders;
			        // Episodes Types
			        var episodeTypes = _episodesDataProvider.Value?.GetEpisodeTypes();
			        if (null != episodeTypes)
			            claimDto.EpisodeTypes = episodeTypes.OrderBy(x => x.SortOrder).ToList();
			        // Claim Prescriptions
			        claimDto.Prescriptions =
			            GetPrescriptionDataByClaim(claimId, "RxDate", "DESC", 1, ic.MaxRowCountForBladeInApp)?.ToList();
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
	}
}