using System;
using System.Collections.Generic;

namespace BridgeportClaims.Data.Dtos
{
	[Serializable]
	public sealed class ClaimDto
	{
		public int ClaimId { get; set; }
		public string Name { get; set; }
		public string ClaimNumber { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string Gender { get; set; }
		public string Carrier { get; set; }
		public string Adjustor { get; set; }
		public string AdjustorPhoneNumber { get; set; }
		public string AdjustorFaxNumber { get; set; }
		public DateTime? EligibilityTermDate { get; set; }
		public string Flex2 { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		public string StateAbbreviation { get; set; }
		public string PostalCode { get; set; }
		public string PatientPhoneNumber { get; set; }
		public DateTime? DateEntered { get; set; }
        public IList<ClaimFlex2Dto> ClaimFlex2s { get; set; }
		public IList<ClaimNoteDto> ClaimNotes { get; set; }
		public IList<EpisodeDto> Episodes { get;set;}
		public IList<PrescriptionDto> Prescriptions { get; set; }
		public IList<PrescriptionNotesDto> PrescriptionNotes { get; set; }
		public IList<AcctPayableDto> AcctPayables { get; set; }
	}
}
