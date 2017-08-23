 using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Data.StoredProcedureExecutors;
using BridgeportClaims.Entities.DomainModels;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;

namespace BridgeportClaims.Data.DataProviders.Claims
{
	public class ClaimsDataProvider : IClaimsDataProvider
	{
		private readonly IStoredProcedureExecutor _storedProcedureExecutor;
		private readonly ISessionFactory _factory;

		public ClaimsDataProvider(ISessionFactory factory,
			IStoredProcedureExecutor storedProcedureExecutor)
		{
			_storedProcedureExecutor = storedProcedureExecutor;
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

		private IList<PrescriptionDto> GetPrescriptionDataByClaim(int claimId)
		{
			var claimIdParam = new SqlParameter
			{
				ParameterName = "ClaimID",
				Value = claimId,
				DbType = DbType.Int32
			};
			return _storedProcedureExecutor.ExecuteMultiResultStoredProcedure<PrescriptionDto>(
				"EXECUTE [dbo].[uspGetPrescriptionDataForClaim] @ClaimID = :ClaimID",
				new List<SqlParameter> {claimIdParam}).ToList();
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
								.Join(session.Query<Claim>(), p => p.PatientId, c => c.Patient.PatientId,
									(p, c) => new { p, c })
								.Where(w => w.c.ClaimId == claimId)
								.Select(w => new ClaimDto
								{
									ClaimId = w.c.ClaimId,
									Name = w.p.FirstName + " " + w.p.LastName,
									Address1 = w.p.Address1,
									Address2 = w.p.Address2,
									Adjustor = null == w.c.Adjustor ? null : w.c.Adjustor.AdjustorName,
									#pragma warning disable IDE0031 // Use null propagationAdjustorFaxNumber = null == w.c.Adjustor ? null : w.c.Adjustor.FaxNumber,
									AdjustorPhoneNumber = null == w.c.Adjustor ? null : w.c.Adjustor.PhoneNumber,
									#pragma warning disable IDE0031 // Use null propagation
									Carrier = null == w.c.Payor ? null : w.c.Payor.GroupName,
									City = w.p.City,
									#pragma warning disable IDE0031 // Use null propagation
									StateAbbreviation = null == w.p.UsState ? null : w.p.UsState.StateCode,
									PostalCode = w.p.PostalCode,
									Flex2 = "PIP", // TODO: remove hard-coded
									#pragma warning disable IDE0031 // Use null propagation
									Gender = null == w.p.Gender ? null : w.p.Gender.GenderName,
									#pragma warning restore IDE0031 // Use null propagation
									DateOfBirth = w.p.DateOfBirth,
									EligibilityTermDate = w.c.TermDate,
									PatientPhoneNumber = w.p.PhoneNumber,
									DateEntered = w.c.DateOfInjury,
									ClaimNumber = w.c.ClaimNumber
								}).ToFuture().SingleOrDefault();
							if (null == claimDto)
								return null;
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
							{
								claimDto.ClaimNotes = claimNoteDto;
							}

							// Claim Episodes
							var episodes = session.CreateSQLQuery(
								  @"SELECT EpisodeId = [e].[EpisodeID]
										 , [Date] = [e].[CreatedDateUTC]
										 , [By] = [u].[FirstName] + ' ' + [u].[LastName]
										 , [e].[Note]
										 , [Type] = [et].[TypeName]
									FROM   [dbo].[Episode] AS [e] 
											INNER JOIN [dbo].[AspNetUsers] AS [u] ON [u].[ID] = [e].[AssignedUserID]
											LEFT JOIN [dbo].[EpisodeType] AS [et] ON [et].[EpisodeTypeID] = [e].[EpisodeTypeID]
									WHERE  [e].[ClaimID] = :ClaimID")
								.SetMaxResults(1000)
								.SetInt32("ClaimID", claimId)
								.SetResultTransformer(Transformers.AliasToBean(typeof(EpisodeDto)))
								.List<EpisodeDto>();
							claimDto.Episodes = episodes;
							var acctPayableDtos = session.CreateSQLQuery(
							  @"SELECT [Date] = [p].[DatePosted]
									 , [p].[CheckNumber]
									 , [p2].[RxNumber]
									 , RxDate = CAST([p2].[DateFilled] AS DATE)
									 , CheckAmount = [p].[AmountPaid]
								FROM   [dbo].[PrescriptionPayment] AS [p]
									   INNER JOIN [dbo].[Prescription] AS [p2] ON [p2].[PrescriptionID] = [p].[PrescriptionID]
								WHERE  [p].[ClaimID] = :ClaimID")
								.SetMaxResults(1000)
								.SetInt32("ClaimID", claimId)
								.SetResultTransformer(Transformers.AliasToBean(typeof(AcctPayableDto)))
								.List<AcctPayableDto>();
							claimDto.AcctPayables = acctPayableDtos;
							// Claim Prescriptions
							claimDto.Prescriptions = 
								GetPrescriptionDataByClaim(claimId)?.OrderByDescending(x => x.RxDate).ToList();
							// Prescription Notes
							var prescriptionNotesDtos = session.CreateSQLQuery(
									@"SELECT DISTINCT [ClaimId] = [a].[ClaimID]
													, [PrescriptionNoteId] = [a].[PrescriptionNoteId]
													, [Date] = [a].[DateFilled]
													, [Type] = [a].[PrescriptionNoteType]
													, [EnteredBy] = [a].[NoteAuthor]
													, [Note] = [a].[NoteText]
													, [NoteUpdatedOn] = [a].[NoteUpdatedOn]
												FROM [dbo].[vwPrescriptionNote] AS a WITH (NOEXPAND)
												WHERE[a].[ClaimID] = :ClaimID
												ORDER BY[a].[NoteUpdatedOn] ASC")
								.SetInt32("ClaimID", claimId)
								.SetMaxResults(1000)
								.SetResultTransformer(Transformers.AliasToBean(typeof(PrescriptionNotesDto)))
								.List<PrescriptionNotesDto>();
							claimDto.PrescriptionNotes = prescriptionNotesDtos;
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
	}
}