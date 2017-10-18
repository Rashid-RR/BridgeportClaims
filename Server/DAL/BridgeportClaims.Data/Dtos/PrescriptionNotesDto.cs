using System;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public sealed class PrescriptionNotesDto
    {
        public int ClaimId { get; set; }
        public int PrescriptionNoteId { get; set; }
        public DateTime RxDate { get; set; }
        public string RxNumber { get; set; }
        public string Type { get; set; }
        public string EnteredBy { get; set; }
        public string Note { get; set; }
        public DateTime? NoteUpdatedOn { get; set; }
        public bool HasDiaryEntry { get; set; }
    }
}
