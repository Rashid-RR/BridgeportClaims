using System.Collections.Generic;

namespace BridgeportClaims.Data.Dtos
{
    public class PrescriptionNoteSaveDto
    {
        public int ClaimId { get; set; }
        public string NoteText { get; set; }
        public int PrescriptionNoteTypeId { get; set; }
        public IList<int> Prescriptions { get; set; }
        public int? PrescriptionNoteId { get; set; }
    }
}