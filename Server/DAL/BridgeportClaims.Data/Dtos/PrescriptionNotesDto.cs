using System;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public sealed class PrescriptionNotesDto
    {
        public int ClaimId { get; set; }
        public int PrescriptionNoteId { get; set; }
        public DateTime? Date { get; set; }
        public string Type { get; set; }
        public string EnteredBy { get; set; }
        public string Note { get; set; }
        public DateTime? NoteUpdatedOn { get; set; }
    }
}
