using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace BridgeportClaims.Data.Dtos
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public sealed class ClaimDto
    {
        public int ClaimId { get; set; }
        public bool isVip { get; set; }
        public string Name { get; set; }
        public bool IsMaxBalance { get; set; }
        public string ClaimNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfInjury { get; set; }
        public string Gender { get; set; }
        public string Carrier { get; set; }
        public string Adjustor { get; set; }
        public string Attorney { get; set; }
        public string AdjustorPhoneNumber { get; set; }
        public string AdjustorExtension { get; set; }
        public string AdjustorFaxNumber { get; set; }
        public DateTime? EligibilityTermDate { get; set; }
        public string Flex2 { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string StateAbbreviation { get; set; }
        public string PostalCode { get; set; }
        public string PatientPhoneNumber { get; set; }
        public int? AdjustorId { get; set; }
        public int? AttorneyId { get; set; }
        public int PayorId { get; set; }
        public int? StateId { get; set; }
        public int PatientGenderId { get; set; }
        public int? ClaimFlex2Id { get; set; }
        public IList<EpisodeTypeDto> EpisodeTypes { get; set; }
        public DateTime? DateEntered { get; set; }
        public IList<GenderDto> Genders { get; set; }
        public IList<UsStateDto> States { get; set; }
        public IList<DocumentTypeDto> DocumentTypes { get; set; }
        public IList<ClaimFlex2Dto> ClaimFlex2s { get; set; }
        public IList<ClaimNoteDto> ClaimNotes { get; set; }
        public IList<EpisodeBladeDto> Episodes { get;set;}
        public IList<PrescriptionStatusDto> PrescriptionStatuses { get; set; }
        public IList<PrescriptionDto> Prescriptions { get; set; }
        public IList<ScriptNoteDto> PrescriptionNotes { get; set; }
        public IList<AcctPayableDto> AcctPayables { get; set; }
        public IList<PrescriptionPaymentsDto> Payments { get; set; }
        public IList<ClaimImageResultDto> Images { get; set; }
        public OutstandingDto Outstanding { get; set; }
    }
}
