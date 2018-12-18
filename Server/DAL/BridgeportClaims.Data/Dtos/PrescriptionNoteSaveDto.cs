using System.Collections.Generic;

namespace BridgeportClaims.Data.Dtos
{
    public class PrescriptionNoteSaveDto
    {
        public int ClaimId { get; set; }
        public string NoteText { get; set; }
        public int PrescriptionNoteTypeId { get; set; }
        public string FollowUpDate { get; set; }
        public bool IsDiaryEntry => null != FollowUpDate;
        public IList<int> Prescriptions { get; set; }
        public int? PrescriptionNoteId { get; set; }
    }
}