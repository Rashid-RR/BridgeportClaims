using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Data.StoredProcedureExecutors;
using BridgeportClaims.Entities.DomainModels;

namespace BridgeportClaims.Data.DataProviders
{
    public class GetClaimsDataProvider : IGetClaimsDataProvider
    {
        private readonly IStoredProcedureExecutor _storedProcedureExecutor;
        private readonly IRepository<Prescription> _prescriptionRepository;
        private readonly IRepository<Claim> _claimRepository;
        private readonly IRepository<Patient> _patientRepository;
        private readonly IRepository<PrescriptionNote> _prescriptionNoteRepository;
        private readonly IRepository<PrescriptionNoteType> _prescriptionNoteTypeRepository;
        private readonly IRepository<Invoice> _invoiceRepository;
        private readonly IRepository<ClaimNote> _claimNoteRepository;
        private readonly IRepository<Episode> _episodeRepository;
        private readonly IRepository<Payment> _paymentRepository;

        public GetClaimsDataProvider(
            IStoredProcedureExecutor storedProcedureExecutor, 
            IRepository<Prescription> prescriptionRepository, 
            IRepository<Claim> claimRepository, 
            IRepository<PrescriptionNote> prescriptionNoteRepository, 
            IRepository<PrescriptionNoteType> prescriptionNoteTypeRepository, 
            IRepository<Invoice> invoiceRepository, 
            IRepository<ClaimNote> claimNoteRepository, 
            IRepository<Episode> episodeRepository, 
            IRepository<Payment> paymentRepository, 
            IRepository<Patient> patientRepository)
        {
            _storedProcedureExecutor = storedProcedureExecutor;
            _prescriptionRepository = prescriptionRepository;
            _claimRepository = claimRepository;
            _prescriptionNoteRepository = prescriptionNoteRepository;
            _prescriptionNoteTypeRepository = prescriptionNoteTypeRepository;
            _invoiceRepository = invoiceRepository;
            _claimNoteRepository = claimNoteRepository;
            _episodeRepository = episodeRepository;
            _paymentRepository = paymentRepository;
            _patientRepository = patientRepository;
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
            var claimDto = (from p in _patientRepository.GetAll()
                join c in _claimRepository.GetAll() on p.PatientId equals c.Patient.PatientId
                where c.ClaimId == claimId
                select

                new ClaimDto
                {
                    Name = p.FirstName + " " + p.LastName,
                    Address1 = p.Address1,
                    Address2 = p.Address2,
                    Adjustor = null == c.Adjustor ? null : c.Adjustor.AdjustorName,
                    AdjustorFaxNumber = null == c.Adjustor ? null : c.Adjustor.FaxNumber,
                    AdjustorPhoneNumber = null == c.Adjustor ? null : c.Adjustor.PhoneNumber,
                    Carrier = null == c.Payor ? null : c.Payor.BillToName,
                    City = p.City,
                    StateAbbreviation = null == p.UsState ? null : p.UsState.StateCode,
                    PostalCode = p.PostalCode,
                    Flex2 = "PIP", // TODO: remove hard-coded
                    Gender = null == p.Gender ? null : p.Gender.GenderName,
                    DateOfBirth = p.DateOfBirth,
                    EligibilityTermDate = c.TermDate,
                    PatientPhoneNumber = p.PhoneNumber,
                    DateEntered = c.DateOfInjury,
                    ClaimNumber = c.ClaimNumber
                }).SingleOrDefault();
            if (null == claimDto)
                throw new ArgumentNullException(nameof(claimDto));
            // Claim Note
            var claimNoteDto = _claimNoteRepository.GetMany(w => (null == w.Claim ? 0 : w.Claim.ClaimId) == claimId).ToList()
                .Select(c => new ClaimNoteDto
                {
                    NoteText = c?.NoteText
                }).FirstOrDefault();
            claimDto.ClaimNote = claimNoteDto;
            // Claim Episodes
            var episodes = _episodeRepository.GetAll()
                .Where(e => e.Claim.ClaimId == claimId)
                .Select(e => new EpisodeDto
                {
                    Date = e.CreatedDate,
                    By = e.AssignUser,
                    Note = e.Note
                }).ToList();
            claimDto.Episodes = episodes;
            // Claim Payments
            var payments = _paymentRepository.GetMany(w => w.Claim.ClaimId == claimId)
                .Select(p => new PaymentDto
                {
                    CheckAmount = p.AmountPaid,
                    CheckNumber = p.CheckNumber,
                    Date = p.CheckDate
                }).ToList();
            claimDto.Payments = payments;
            // Claim Prescriptions
            var prescriptions = _prescriptionRepository.GetMany(w => w.Claim.ClaimId == claimId)
                .Join(_invoiceRepository.GetAll(), p => p.Claim.ClaimId, i => i.Claim.ClaimId,
                    (p, i) => new PrescriptionDto
                    {
                        RxDate = p.RefillDate,
                        AmountPaid = p.PayableAmount,
                        RxNumber = p.RxNumber,
                        LabelName = p.LabelName,
                        BillTo = null == i.Payor ? string.Empty : i.Payor.BillToName,
                        InvoiceAmount = i.Amount,
                        InvoiceDate = i.InvoiceDate,
                        InvoiceNumber = i.InvoiceNumber,
                        Outstanding = i.Amount // TODO: Fix
                    }).ToList();
            claimDto.Prescriptions = prescriptions;
            // Prescription Notes
            var prescriptionNotes = _prescriptionRepository.GetAll()
                .Join(_prescriptionNoteRepository.GetAll(), p => p.PrescriptionId, pn => pn.Prescription.PrescriptionId,
                    (p, pn) => new {p, pn})
                .Join(_prescriptionNoteTypeRepository.GetAll(),
                    pnl => pnl.pn.PrescriptionNoteType.PrescriptionNoteTypeId,
                    pnt => pnt.PrescriptionNoteTypeId, (x, pnt) => new {y = x, pnt})
                .Where(w => w.y.p.Claim.ClaimId == claimId)
                .Select(s => new PrescriptionNotesDto
                {
                    Date = s.y.pn.CreatedOn,
                    EnteredBy = s.y.pn.AspNetUsers.Email,
                    Note = s.y.pn.NoteText,
                    Type = s.pnt.TypeName
                }).ToList();
            claimDto.PrescriptionNotes = prescriptionNotes;
            return claimDto;
        }
    }
}
