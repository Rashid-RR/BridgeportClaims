using System.Threading.Tasks;
using BridgeportClaims.Business.Models;

namespace BridgeportClaims.Data.DataProviders.PrescriptionNotes
{
    public interface IPrescriptionNotesDataProvider
    {
        void AddOrUpdatePrescriptionNoteAsync(PrescriptionNoteSaveModel model, string userId);
    }
}