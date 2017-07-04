using System.Threading.Tasks;
using BridgeportClaims.Business.Models;

namespace BridgeportClaims.Data.DataProviders.PrescriptionNotes
{
    public interface IPrescriptionNotesDataProvider
    {
        Task AddOrUpdatePrescriptionNoteAsync(PrescriptionNoteSaveModel model, string userId);
    }
}