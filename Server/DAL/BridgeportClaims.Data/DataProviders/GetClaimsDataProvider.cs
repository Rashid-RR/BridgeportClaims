using System;
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

namespace BridgeportClaims.Data.DataProviders
{
    public class GetClaimsDataProvider : IGetClaimsDataProvider
    {
        private readonly IStoredProcedureExecutor _storedProcedureExecutor;
        private readonly ISessionFactory _factory;

        public GetClaimsDataProvider(ISessionFactory factory,
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
                                    (p, c) => new {p, c})
                                .Where(w => w.c.ClaimId == claimId)
                                .Select(w => new ClaimDto
                                {
                                    Name = w.p.FirstName + " " + w.p.LastName,
                                    Address1 = w.p.Address1,
                                    Address2 = w.p.Address2,
                                    Adjustor = null == w.c.Adjustor ? null : w.c.Adjustor.AdjustorName,
                                    AdjustorFaxNumber = null == w.c.Adjustor ? null : w.c.Adjustor.FaxNumber,
                                    AdjustorPhoneNumber = null == w.c.Adjustor ? null : w.c.Adjustor.PhoneNumber,
                                    Carrier = null == w.c.Payor ? null : w.c.Payor.BillToName,
                                    City = w.p.City,
                                    StateAbbreviation = null == w.p.UsState ? null : w.p.UsState.StateCode,
                                    PostalCode = w.p.PostalCode,
                                    Flex2 = "PIP", // TODO: remove hard-coded
                                    Gender = null == w.p.Gender ? null : w.p.Gender.GenderName,
                                    DateOfBirth = w.p.DateOfBirth,
                                    EligibilityTermDate = w.c.TermDate,
                                    PatientPhoneNumber = w.p.PhoneNumber,
                                    DateEntered = w.c.DateOfInjury,
                                    ClaimNumber = w.c.ClaimNumber
                                }).ToFuture().SingleOrDefault();
                            if (null == claimDto)
                                throw new ArgumentNullException(nameof(claimDto));
                            // Claim Note
                            var claimNoteDto = session.Query<ClaimNote>().Where(w =>
                                    (null == w.Claim ? 0 : w.Claim.ClaimId) == claimId)
                                    .ToFuture()
                                .ToList()
                                .Select(c => new ClaimNoteDto
                                {
                                    NoteText = c?.NoteText
                                }).FirstOrDefault();
                            if (null != claimNoteDto)
                            {
                                claimDto.ClaimNote = claimNoteDto;
                            }

                            // Claim Episodes
                            var episodes = session.Query<Episode>()
                                .Where(e => e.Claim.ClaimId == claimId)
                                .Select(e => new EpisodeDto
                                {
                                    Date = e.CreatedDate,
                                    By = e.AssignUser,
                                    Note = e.Note
                                }).ToFuture().ToList();
                            claimDto.Episodes = episodes;
                            // Claim Payments
                            var payments = session.Query<Payment>().Where(w => w.Claim.ClaimId == claimId)
                                .Select(p => new PaymentDto
                                {
                                    CheckAmount = p.AmountPaid,
                                    CheckNumber = p.CheckNumber,
                                    Date = p.CheckDate
                                }).ToList();
                            claimDto.Payments = payments;
                            // Claim Prescriptions
                            var prescriptions = session.Query<Prescription>().Where(w => w.ClaimId == claimId)
                                .Join(session.Query<Invoice>(), p => p.Claim.ClaimId, i => i.Claim.ClaimId,
                                    (p, i) => new PrescriptionDto
                                    {
                                        RxDate = p.RefillDate,
                                        AmountPaid = p.PayableAmount,
                                        RxNumber = p.RxNumber,
                                        LabelName = p.LabelName,
                                        BillTo = null == i.Payor ? null : i.Payor.BillToName,
                                        InvoiceAmount = i.Amount,
                                        InvoiceDate = i.InvoiceDate,
                                        InvoiceNumber = i.InvoiceNumber,
                                        Outstanding = i.Amount // TODO: Fix
                                    }).ToFuture().ToList();
                            claimDto.Prescriptions = prescriptions;
                            // Prescription Notes
                            var prescriptionNotesDtos = session.CreateSQLQuery(
                                    "SELECT [a].[DateFilled] [Date]" +
                                    ", [a].[PrescriptionNoteType] [Type]" +
                                    ", [a].[NoteAuthor] [EnteredBy]" +
                                    ", [a].[NoteText] [Note] " +
                                    "FROM [dbo].[vwPrescriptionNote] AS a WITH ( NOEXPAND ) " +
                                    "WHERE [a].[ClaimID] = :ClaimID " +
                                    "ORDER BY [a].[NoteUpdatedOn]")
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