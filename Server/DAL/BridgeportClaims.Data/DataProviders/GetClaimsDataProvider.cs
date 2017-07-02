using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using BridgeportClaims.Common.Expressions;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Data.Mappings.Views;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Data.StoredProcedureExecutors;
using BridgeportClaims.Entities.DomainModels;
using BridgeportClaims.Entities.DomainModels.Views;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Criterion.Lambda;
using NHibernate.Linq;
using NHibernate.Transform;

namespace BridgeportClaims.Data.DataProviders
{
    public class GetClaimsDataProvider : BaseRepository, IGetClaimsDataProvider
    {
        private readonly IStoredProcedureExecutor _storedProcedureExecutor;
        private readonly IRepository<Prescription> _prescriptionRepository;
        private readonly IRepository<Claim> _claimRepository;
        private readonly IRepository<Patient> _patientRepository;
        private readonly IRepository<PrescriptionNoteType> _prescriptionNoteTypeRepository;
        private readonly IRepository<Invoice> _invoiceRepository;
        private readonly IRepository<ClaimNote> _claimNoteRepository;
        private readonly IRepository<Episode> _episodeRepository;
        private readonly IRepository<Payment> _paymentRepository;
        private readonly IRepository<VwPrescriptionNote> _vwPrescriptionNoteRepository;

        public GetClaimsDataProvider(ISession session,
            IStoredProcedureExecutor storedProcedureExecutor, 
            IRepository<Prescription> prescriptionRepository, 
            IRepository<Claim> claimRepository,
            IRepository<PrescriptionNoteType> prescriptionNoteTypeRepository, 
            IRepository<Invoice> invoiceRepository, 
            IRepository<ClaimNote> claimNoteRepository, 
            IRepository<Episode> episodeRepository, 
            IRepository<Payment> paymentRepository, 
            IRepository<Patient> patientRepository, 
            IRepository<VwPrescriptionNote> vwPrescriptionNoteRepository) : base(session)
        {
            _storedProcedureExecutor = storedProcedureExecutor;
            _prescriptionRepository = prescriptionRepository;
            _claimRepository = claimRepository;
            _prescriptionNoteTypeRepository = prescriptionNoteTypeRepository;
            _invoiceRepository = invoiceRepository;
            _claimNoteRepository = claimNoteRepository;
            _episodeRepository = episodeRepository;
            _paymentRepository = paymentRepository;
            _patientRepository = patientRepository;
            _vwPrescriptionNoteRepository = vwPrescriptionNoteRepository;
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
            var claim = _claimRepository.Get(claimId);
            if (null == claim)
                throw new Exception($"No claim found with with Claim ID {claimId}");
            var claimDto = Session.Query<Patient>()
                .Join(Session.Query<Claim>(), p => p.PatientId, c => c.Patient.PatientId, (p, c) => new {p, c})
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
                }).SingleOrDefault();
            if (null == claimDto)
                throw new ArgumentNullException(nameof(claimDto));
            // Claim Note
            var claimNoteDto = Session.Query<ClaimNote>().Where(w => 
                (null == w.Claim ? 0 : w.Claim.ClaimId) == claimId)
                .ToList()
                .Select(c => new ClaimNoteDto
                {
                    NoteText = c?.NoteText
                }).FirstOrDefault();
            if (null != claimNoteDto)
            {
                claimDto.ClaimNotes = new List<ClaimNoteDto> {claimNoteDto};
            }
            // Claim Episodes
            var episodes = Session.Query<Episode>()
                .Where(e => e.Claim.ClaimId == claimId)
                .Select(e => new EpisodeDto
                {
                    Date = e.CreatedDate,
                    By = e.AssignUser,
                    Note = e.Note
                }).ToList();
            claimDto.Episodes = episodes;
            // Claim Payments
            var payments = Session.Query<Payment>().Where(w => w.Claim.ClaimId == claimId)
                .Select(p => new PaymentDto
                {
                    CheckAmount = p.AmountPaid,
                    CheckNumber = p.CheckNumber,
                    Date = p.CheckDate
                }).ToList();
            claimDto.Payments = payments;
            // Claim Prescriptions
            var prescriptions = Session.Query<Prescription>().Where(w => w.ClaimId == claimId)
                .Join(Session.Query<Invoice>(), p => p.Claim.ClaimId, i => i.Claim.ClaimId,
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
                    }).ToList();
            claimDto.Prescriptions = prescriptions;
            // Prescription Notes
            var prescriptionNotes = Session.CreateQuery(@"SELECT [a].[DateFilled] [Date]                                                               , [a].[DateFilled] [Date]
                                                                , [a].[PrescriptionNoteType] [Type]                                         
                                                                , [a].[NoteAuthor] [EnteredBy]
                                                                , [a].[NoteText] [Note]
                                                            FROM[dbo].[vwPrescriptionNote] AS a WITH(NOEXPAND)
                                                            ORDER BY [a].[NoteUpdatedOn]")
                .List<PrescriptionNotesDto>();
            claimDto.PrescriptionNotes = prescriptionNotes;
            return claimDto;
        }

        // Data fill the drop down list for the Prescription Note Types.
        private IList<KeyValuePair<int, string>> GetPrescriptionNoteTypes()
        {
            var query = _prescriptionNoteTypeRepository?.GetAll()?.ToList();
            if (null == query|| !query.Any())
                return null;
            return query.Select(pair => new KeyValuePair<int, string>(
                pair.PrescriptionNoteTypeId, pair.TypeName)).ToList();
        }
    }
}
