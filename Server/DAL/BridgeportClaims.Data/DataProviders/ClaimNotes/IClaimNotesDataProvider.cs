using System.Collections.Generic;
using System.Threading.Tasks;

namespace BridgeportClaims.Data.DataProviders.ClaimNotes
{
    public interface IClaimNotesDataProvider
    {
        Task<IList<KeyValuePair<int, string>>> GetClaimNoteTypesAsync();
        void AddOrUpdateNote(int claimId, string note, string enteredByUserId, int? noteTypeId);
        void DeleteClaimNote(int claimId);
    }
}