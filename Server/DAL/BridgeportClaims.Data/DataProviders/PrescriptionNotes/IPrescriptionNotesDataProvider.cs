using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.PrescriptionNotes
{
    public interface IPrescriptionNotesDataProvider
    {
        IList<PrescriptionNotesDto> GetPrescriptionNotesByPrescriptionId(int prescriptionId);
        void AddOrUpdatePrescriptionNote(PrescriptionNoteSaveDto dto, string userId);
    }
}