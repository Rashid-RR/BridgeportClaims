using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.DataProviders.ClaimImages;
using BridgeportClaims.Data.DataProviders.Payments;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Data.Enums;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Data.StoredProcedureExecutors;
using BridgeportClaims.Entities.DomainModels;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;
using c = BridgeportClaims.Common.StringConstants.Constants;


namespace BridgeportClaims.Data.DataProviders.Claims
{
	public class ClaimsDataProvider : IClaimsDataProvider
	{
		private readonly IStoredProcedureExecutor _storedProcedureExecutor;
		private readonly ISessionFactory _factory;
		private readonly IPaymentsDataProvider _paymentsDataProvider;
		private readonly IRepository<Claim> _claimRepository;
		private readonly IRepository<ClaimFlex2> _claimFlex2Repository;
	    private readonly IClaimImageProvider _claimImageProvider;

		public ClaimsDataProvider(ISessionFactory factory, 
			IStoredProcedureExecutor storedProcedureExecutor, 
			IRepository<Claim> claimRepository, 
			IRepository<ClaimFlex2> claimFlex2Repository, 
			IPaymentsDataProvider paymentsDataProvider, 
            IClaimImageProvider claimImageProvider)
		{
			_storedProcedureExecutor = storedProcedureExecutor;
			_claimRepository = claimRepository;
			_claimFlex2Repository = claimFlex2Repository;
			_paymentsDataProvider = paymentsDataProvider;
		    _claimImageProvider = claimImageProvider;
		    _factory = factory;
		}

		public IList<GetClaimsSearchResults> GetClaimsData(string claimNumber, string firstName, string lastName,
			string rxNumber, string invoiceNumber)
		{
			var claimNumberParam = new SqlParameter
			{
				ParameterName = "ClaimNumber",
				Value = claimNumber,
				DbType = DbType.String
			};

			var firstNameParam = new SqlParameter
			{
				ParameterName = "FirstName",
				Value = firstName,
				DbType = DbType.String
			};

			var lastNameParam = new SqlParameter
			{
				ParameterName = "LastName",
				Value = lastName,
				DbType = DbType.String
			};

			var rxNumberParam = new SqlParameter
			{
				ParameterName = "RxNumber",
				Value = rxNumber,
				DbType = DbType.String
			};

			var invoiceNumberParam = new SqlParameter
			{
				ParameterName = "InvoiceNumber",
				Value = invoiceNumber,
				DbType = DbType.String
			};

			var retVal = _storedProcedureExecutor.ExecuteMultiResultStoredProcedure<GetClaimsSearchResults>
			("EXECUTE dbo.uspGetClaimsSearchResults @ClaimNumber = :ClaimNumber, @FirstName = :FirstName, " +
			 "@LastName = :LastName, @RxNumber = :RxNumber, @InvoiceNumber = :InvoiceNumber",
				new List<SqlParameter>
				{
					claimNumberParam,
					firstNameParam,
					lastNameParam,
					rxNumberParam,
					invoiceNumberParam
				}).ToList();
			return retVal;
		}

		public IList<PrescriptionDto> GetPrescriptionDataByClaim(int claimId, string sort, string direction, int page,
			int pageSize)
		{
			var claimIdParam = new SqlParameter
			{
				ParameterName = "ClaimID",
				Value = claimId,
				DbType = DbType.Int32
			};
			var sortParam = new SqlParameter
			{
				ParameterName = "SortColumn",
				Value = sort,
				DbType = DbType.String
			};
			var sortDirectionParam = new SqlParameter
			{
				ParameterName = "SortDirection",
				Value = direction,
				DbType = DbType.String
			};
			var pageNumberParam = new SqlParameter
			{
				ParameterName = "PageNumber",
				Value = page,
				DbType = DbType.Int32
			};
			var pageSizeParam = new SqlParameter
			{
				ParameterName = "PageSize",
				Value = pageSize,
				DbType = DbType.Int32
			};
			return _storedProcedureExecutor.ExecuteMultiResultStoredProcedure<PrescriptionDto>(
					"EXECUTE [dbo].[uspGetPrescriptionDataForClaim] @ClaimID = :ClaimID, @SortColumn = :SortColumn, @SortDirection " +
					"= :SortDirection, @PageNumber = :PageNumber, @PageSize = :PageSize",
					new List<SqlParameter> {claimIdParam, sortParam, sortDirectionParam, pageNumberParam, pageSizeParam})
				.ToList();
		}

		public ClaimDto GetClaimsDataByClaimId(int claimId)
		{
			return DisposableService.Using(() => _factory.OpenSession(), session =>
			{
				return DisposableService.Using(() => session.BeginTransaction(IsolationLevel.ReadCommitted),
					tx =>
					{
						try
						{
							var claimDto = session.Query<Patient>()
								.Join(session.Query<Claim>(), p => p.PatientId, c => c.Patient.PatientId, (p, c) => new {p, c})
								.Where(w => w.c.ClaimId == claimId)
								.Select(s => new ClaimDto
								{
									ClaimId = s.c.ClaimId,
									Name = s.p.FirstName + " " + s.p.LastName,
									Address1 = s.p.Address1,
									Address2 = s.p.Address2,
									Adjustor = null == s.c.Adjustor ? null : s.c.Adjustor.AdjustorName,
									AdjustorPhoneNumber = null == s.c.Adjustor ? null : s.c.Adjustor.PhoneNumber,
									Carrier = null == s.c.Payor ? null : s.c.Payor.GroupName,
									City = s.p.City,
									StateAbbreviation = null == s.p.UsState ? null : s.p.UsState.StateCode,
									PostalCode = s.p.PostalCode,
									Flex2 = null != s.c.ClaimFlex2 ? s.c.ClaimFlex2.Flex2 : null,
									Gender = null == s.p.Gender ? null : s.p.Gender.GenderName,
									DateOfBirth = s.p.DateOfBirth,
									EligibilityTermDate = s.c.TermDate,
									PatientPhoneNumber = s.p.PhoneNumber,
									DateEntered = s.c.DateOfInjury,
									ClaimNumber = s.c.ClaimNumber
								}).SingleOrDefault();
							if (null == claimDto)
								return null;
							// ClaimFlex2 Drop-Down Values
							var claimFlex2Dto = session.CreateSQLQuery("SELECT ClaimFlex2ID ClaimFlex2Id, Flex2 FROM dbo.ClaimFlex2")
								.SetMaxResults(100)
								.SetResultTransformer(Transformers.AliasToBean(typeof(ClaimFlex2Dto)))
								.List<ClaimFlex2Dto>();
							if (null != claimFlex2Dto)
								claimDto.ClaimFlex2s = claimFlex2Dto;
							// Claim Note
							var claimNoteDto = session.CreateSQLQuery(
									@"SELECT cnt.[TypeName] NoteType, [cn]. [NoteText]
										FROM   [dbo].[ClaimNote] AS [cn]
												LEFT JOIN [dbo].[ClaimNoteType] AS [cnt] 
												ON [cnt].[ClaimNoteTypeID] = [cn].[ClaimNoteTypeID]
										WHERE  [cn].[ClaimID] = :ClaimID")
								.SetInt32("ClaimID", claimId)
								.SetMaxResults(1)
								.SetResultTransformer(Transformers.AliasToBean(typeof(ClaimNoteDto)))
								.List<ClaimNoteDto>();
							if (null != claimNoteDto)
								claimDto.ClaimNotes = claimNoteDto;
							// Claim Episodes
							var episodes = session.CreateSQLQuery(
								  @"SELECT EpisodeId = [e].[EpisodeID]
										 , [Date] = [e].[CreatedDateUTC]
										 , [By] = [u].[FirstName] + ' ' + [u].[LastName]
										 , [e].[Role]										 
										 , [e].[Note]
									FROM   [dbo].[Episode] AS [e] 
											INNER JOIN [dbo].[AspNetUsers] AS [u] ON [u].[ID] = [e].[AssignedUserID]
											LEFT JOIN [dbo].[EpisodeType] AS [et] ON [et].[EpisodeTypeID] = [e].[EpisodeTypeID]
									WHERE  [e].[ClaimID] = :ClaimID")
								.SetMaxResults(5000)
								.SetInt32("ClaimID", claimId)
								.SetResultTransformer(Transformers.AliasToBean(typeof(EpisodeDto)))
								.List<EpisodeDto>();
							if (null != episodes)
								claimDto.Episodes = episodes.OrderByDescending(x => x.Date).ToList();
							var acctPayableDtos = session.CreateSQLQuery(
							  @"SELECT [Date] = [p].[DatePosted]
									 , [p].[CheckNumber]
									 , [p2].[RxNumber]
									 , RxDate = CAST([p2].[DateFilled] AS DATE)
									 , CheckAmount = [p].[AmountPaid]
								FROM   [dbo].[PrescriptionPayment] AS [p]
									   INNER JOIN [dbo].[Prescription] AS [p2] ON [p2].[PrescriptionID] = [p].[PrescriptionID]
								WHERE  [p2].[ClaimID] = :ClaimID")
								.SetMaxResults(1000)
								.SetInt32("ClaimID", claimId)
								.SetResultTransformer(Transformers.AliasToBean(typeof(AcctPayableDto)))
								.List<AcctPayableDto>();
							claimDto.AcctPayables = acctPayableDtos;
							// Prescription Statuses
							var prescriptionStatuses = session.CreateSQLQuery("SELECT ps.PrescriptionStatusID PrescriptionStatusId, ps.StatusName FROM dbo.PrescriptionStatus AS ps")
								.SetMaxResults(100)
								.SetResultTransformer(Transformers.AliasToBean(typeof(PrescriptionStatusDto)))
								.List<PrescriptionStatusDto>();
							if (null != prescriptionStatuses)
								claimDto.PrescriptionStatuses = prescriptionStatuses;
							// Payments
							var payments = _paymentsDataProvider.GetPrescriptionPaymentsDtos(claimId, "RxDate", "DESC", 1, 5000, "RxNumber", "ASC");
							if (null != payments)
								claimDto.Payments = payments;
							// Claim Prescriptions
							claimDto.Prescriptions = GetPrescriptionDataByClaim(claimId, "RxDate", "DESC", 1, 5000)?.ToList();
							// Prescription Notes
							var prescriptionNotesDtos = session.CreateSQLQuery(
									 @"SELECT DISTINCT
													[ClaimId]              = [a].[ClaimID]
													, [PrescriptionNoteId] = [a].[PrescriptionNoteID]
													, RxDate               = a.DateFilled
													, a.RxNumber
													, [Type]               = [a].[PrescriptionNoteType]
													, [EnteredBy]          = [a].[NoteAuthor]
													, [Note]               = [a].[NoteText]
													, [NoteUpdatedOn]      = [a].[NoteUpdatedOn]
													, HasDiaryEntry		   = CAST(CASE WHEN d.DiaryID IS NOT NULL THEN 1 ELSE 0 END AS BIT)
										FROM        [dbo].[vwPrescriptionNote] AS a WITH (NOEXPAND)
													LEFT JOIN dbo.Diary AS d ON d.PrescriptionNoteID = a.PrescriptionNoteID AND d.DateResolved IS NULL
										WHERE       [a].[ClaimID] = :ClaimID
										ORDER BY    a.DateFilled DESC, a.RxNumber ASC")
								.SetInt32("ClaimID", claimId)
								.SetMaxResults(5000)
								.SetResultTransformer(Transformers.AliasToBean(typeof(PrescriptionNotesDto)))
								.List<PrescriptionNotesDto>();
							var scriptNotesDtos = prescriptionNotesDtos?.GroupBy(r => new
								{
									r.ClaimId,
									r.PrescriptionNoteId,
									r.Type,
									r.EnteredBy,
									r.Note,
									r.NoteUpdatedOn,
									r.HasDiaryEntry
								}).Select(gcs => new ScriptNoteDto
								{
									ClaimId = gcs.Key.ClaimId,
									Scripts = gcs.Select(x => new ScriptDto {RxNumber = x.RxNumber, RxDate = x.RxDate})
										.OrderByDescending(x => x.RxDate)
										.ThenBy(x => x.RxNumber)
										.ToList(),
									EnteredBy = gcs.Key.EnteredBy,
									HasDiaryEntry = gcs.Key.HasDiaryEntry,
									Note = gcs.Key.Note,
									NoteUpdatedOn = gcs.Key.NoteUpdatedOn,
									PrescriptionNoteId = gcs.Key.PrescriptionNoteId,
									Type = gcs.Key.Type
								}).ToList();
							claimDto.PrescriptionNotes = scriptNotesDtos;
						    var imageResults = _claimImageProvider.GetClaimImages("Created", "DESC", 1, 500);
						    claimDto.Images = imageResults.ClaimImages;
							if (tx.IsActive)
								tx.Commit();
							return claimDto;
						}
						catch
						{
							if (tx.IsActive)
								tx.Rollback();
							throw;
						}
					});
			});
		}

		public EntityOperation AddOrUpdateFlex2(int claimId, int claimFlex2Id)
		{
			var claim = _claimRepository.Get(claimId);
			if (null == claim)
				throw new ArgumentNullException(nameof(claim));
			var claimFlex2 = _claimFlex2Repository.Get(claimFlex2Id);
			var op = null == claim.ClaimFlex2 ? EntityOperation.Add : EntityOperation.Update;
			claim.ClaimFlex2 = claimFlex2 ?? throw new ArgumentNullException(nameof(claimFlex2));
			claim.UpdatedOnUtc = DateTime.UtcNow;
			_claimRepository.Update(claim);
			return op;
		}
	}
}