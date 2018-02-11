﻿using System;
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
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Data.StoredProcedureExecutors;
using BridgeportClaims.Entities.DomainModels;
using NHibernate;
using NHibernate.Transform;
using NLog;
using c = BridgeportClaims.Common.StringConstants.Constants;
using cs = BridgeportClaims.Common.Config.ConfigService;


namespace BridgeportClaims.Data.DataProviders.Claims
{
	public class ClaimsDataProvider : IClaimsDataProvider
	{
		private readonly IStoredProcedureExecutor _storedProcedureExecutor;
	    private readonly IEpisodesDataProvider _episodesDataProvider;
        private readonly ISessionFactory _factory;
		private readonly IPaymentsDataProvider _paymentsDataProvider;
		private readonly IRepository<Claim> _claimRepository;
		private readonly IRepository<ClaimFlex2> _claimFlex2Repository;
	    private readonly IRepository<UsState> _usStateRepository;
	    private readonly IClaimImageProvider _claimImageProvider;
	    private readonly IRepository<AspNetUsers> _usersRepository;
	    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ClaimsDataProvider(ISessionFactory factory, 
			IStoredProcedureExecutor storedProcedureExecutor, 
			IRepository<Claim> claimRepository, 
			IRepository<ClaimFlex2> claimFlex2Repository, 
			IPaymentsDataProvider paymentsDataProvider, 
            IClaimImageProvider claimImageProvider, 
            IRepository<UsState> usStateRepository, 
            IRepository<AspNetUsers> usersRepository, 
            IEpisodesDataProvider episodesDataProvider)
		{
			_storedProcedureExecutor = storedProcedureExecutor;
			_claimRepository = claimRepository;
			_claimFlex2Repository = claimFlex2Repository;
			_paymentsDataProvider = paymentsDataProvider;
		    _claimImageProvider = claimImageProvider;
		    _usStateRepository = usStateRepository;
		    _usersRepository = usersRepository;
		    _episodesDataProvider = episodesDataProvider;
		    _factory = factory;
		}

	    private IList<UsStateDto> GetUsStates()
	    {
	        var states = _usStateRepository.GetAll()?.Select(s => new UsStateDto
	        {
	            StateId = s.StateId,
	            StateName = s.StateCode + " - " + s.StateName
	        });
	        return states?.OrderBy(x => x.StateName).ToList();
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


	    public EntityOperation AddOrUpdateFlex2(int claimId, int claimFlex2Id, string modifiedByUserId)
	    {
	        var claim = _claimRepository.Get(claimId);
	        if (null == claim)
	            throw new ArgumentNullException(nameof(claim));
	        var claimFlex2 = _claimFlex2Repository.Get(claimFlex2Id);
	        var op = null == claim.ClaimFlex2 ? EntityOperation.Add : EntityOperation.Update;
	        claim.ClaimFlex2 = claimFlex2 ?? throw new ArgumentNullException(nameof(claimFlex2));
	        claim.UpdatedOnUtc = DateTime.UtcNow;
	        claim.ModifiedByUserId = _usersRepository.Get(modifiedByUserId);
            _claimRepository.Update(claim);
	        return op;
	    }

	    public IList<EpisodeBladeDto> GetEpisodesBlade(int claimId, string sortColumn, string sortDirection) =>
	        DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
	        {
	            return DisposableService.Using(() => new SqlCommand("[dbo].[uspGetEpisodesBlade]", conn), cmd =>
	            {
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
	                    var categoryOrdinal = reader.GetOrdinal("Category");
	                    var resolvedOrdinal = reader.GetOrdinal("Resolved");
	                    var noteCountOrdinal = reader.GetOrdinal("NoteCount");
                        var result = new EpisodeBladeDto
	                    {
	                        Id = !reader.IsDBNull(idOrdinal) ? reader.GetInt32(idOrdinal) : default (int),
	                        Created = !reader.IsDBNull(createdOrdinal) ? reader.GetDateTime(createdOrdinal) : DateTime.UtcNow.ToMountainTime(),
	                        Owner = !reader.IsDBNull(ownerOrdinal) ? reader.GetString(ownerOrdinal) : string.Empty,
	                        Type = !reader.IsDBNull(typeOrdinal) ? reader.GetString(typeOrdinal) : string.Empty,
	                        Role = !reader.IsDBNull(roleOrdinal) ? reader.GetString(roleOrdinal) : string.Empty,
	                        Pharmacy = !reader.IsDBNull(pharmacyOrdinal) ? reader.GetString(pharmacyOrdinal) : string.Empty,
	                        RxNumber = !reader.IsDBNull(rxNumberOrdinal) ? reader.GetString(rxNumberOrdinal) : string.Empty,
	                        Category = !reader.IsDBNull(categoryOrdinal) ? reader.GetString(categoryOrdinal) : string.Empty,
	                        Resolved = !reader.IsDBNull(resolvedOrdinal) && reader.GetBoolean(resolvedOrdinal),
	                        NoteCount = !reader.IsDBNull(noteCountOrdinal) ? reader.GetInt32(noteCountOrdinal) : default (int)
                        };
                        retVal.Add(result);
	                });
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
	                return retVal;
	            });
	        });

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
                                    City = s.p.City,
                                    StateAbbreviation = null == s.p.StateId ? null : s.p.StateId.StateCode,
								    PostalCode = s.p.PostalCode,
                                    Adjustor = null == s.c.Adjustor ? null : s.c.Adjustor.AdjustorName,
									AdjustorPhoneNumber = null == s.c.Adjustor ? null : s.c.Adjustor.PhoneNumber,
									Carrier = null == s.c.Payor ? null : s.c.Payor.GroupName,									
									Flex2 = null != s.c.ClaimFlex2 ? s.c.ClaimFlex2.Flex2 : null,
									Gender = null == s.p.Gender ? null : s.p.Gender.GenderName,
									DateOfBirth = s.p.DateOfBirth,
                                    DateOfInjury = s.c.DateOfInjury,
									EligibilityTermDate = s.c.TermDate,
									PatientPhoneNumber = s.p.PhoneNumber,
									DateEntered = s.c.DateOfInjury,
									ClaimNumber = s.c.ClaimNumber,
                                    AdjustorId = null == s.c.Adjustor ? (int?) null : s.c.Adjustor.AdjustorId,
                                    PayorId = s.c.Payor.PayorId,
                                    StateId = null == s.p.StateId ? (int?) null : s.p.StateId.StateId,
                                    PatientGenderId = s.p.Gender.GenderId,
								    ClaimFlex2Id = null == s.c.ClaimFlex2 ? (int?) null : s.c.ClaimFlex2.ClaimFlex2Id
                                }).SingleOrDefault();
							if (null == claimDto)
								return null;
							// ClaimFlex2 Drop-Down Values
							var claimFlex2Dto = session.CreateSQLQuery("SELECT ClaimFlex2ID ClaimFlex2Id, Flex2 FROM dbo.ClaimFlex2")
								.SetMaxResults(100)
								.SetResultTransformer(Transformers.AliasToBean(typeof(ClaimFlex2Dto)))
								.List<ClaimFlex2Dto>();
						    if (null != claimFlex2Dto)
						        claimDto.ClaimFlex2s = claimFlex2Dto.OrderBy(x => x.Flex2).ToList();
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
						    var episodes = GetEpisodesBlade(claimId, "Created", "DESC");
						    if (null != episodes)
						        claimDto.Episodes = episodes;
						    var documentTypes = session.CreateSQLQuery(@"SELECT  DocumentTypeId = [dt].[DocumentTypeID]
                                                                               , [dt].[TypeName]
                                                                         FROM    [dbo].[DocumentType] AS [dt]")
						        .SetMaxResults(5000)
						        .SetResultTransformer(Transformers.AliasToBean(typeof(DocumentTypeDto)))
						        .List<DocumentTypeDto>();
						    if (null != documentTypes)
						        claimDto.DocumentTypes = documentTypes.OrderBy(x => x.TypeName).ToList();
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
                            // U.S. States
						    var states = GetUsStates();
						    if (null != states)
						        claimDto.States = states;
							// Prescription Statuses
							var prescriptionStatuses = session.CreateSQLQuery("SELECT ps.PrescriptionStatusID PrescriptionStatusId, ps.StatusName FROM dbo.PrescriptionStatus AS ps")
								.SetMaxResults(100)
								.SetResultTransformer(Transformers.AliasToBean(typeof(PrescriptionStatusDto)))
								.List<PrescriptionStatusDto>();
						    if (null != prescriptionStatuses)
						        claimDto.PrescriptionStatuses = prescriptionStatuses.OrderBy(x => x.StatusName).ToList();
							// Payments
							var payments = _paymentsDataProvider.GetPrescriptionPaymentsDtos(claimId, "RxDate", "DESC", 1, 5000, "RxNumber", "ASC");
							if (null != payments)
								claimDto.Payments = payments;
                            // Genders
						    var genders = session
						        .CreateSQLQuery("SELECT GenderId = [g].[GenderID], [g].[GenderName] FROM [dbo].[Gender] AS [g]")
						        .SetMaxResults(30)
						        .SetResultTransformer(Transformers.AliasToBean(typeof(GenderDto)))
						        .List<GenderDto>();
						    if (null != genders)
						        claimDto.Genders = genders;
                            // Episodes Types
						    var episodeTypes = _episodesDataProvider?.GetEpisodeTypes();
						    if (null != episodeTypes)
						        claimDto.EpisodeTypes = episodeTypes.OrderBy(x => x.SortOrder).ToList();
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
						    var imageResults = _claimImageProvider.GetClaimImages(claimId, "Created", "DESC", 1, 5000);
						    claimDto.Images = imageResults.ClaimImages;
							if (tx.IsActive)
								tx.Commit();
							return claimDto;
						}
						catch (Exception ex)
						{
						    Logger.Error(ex);
                            if (tx.IsActive)
								tx.Rollback();
							throw;
						}
					});
			});
		}
	}
}