using System.Collections.Generic;
using BridgeportClaims.Business.Models;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.PrescriptionNotes
{
    public interface IPrescriptionNotesDataProvider
    {
        IList<PrescriptionNotesDto> GetPrescriptionNotesByPrescriptionId(int prescriptionId);
        void AddOrUpdatePrescriptionNote(PrescriptionNoteSaveModel model, string userId);
    }
}