using System.Collections.Generic;

namespace BridgeportClaims.Data.DataProviders.ClaimNotes
{
    public interface IClaimNotesDataProvider
    {
        IList<KeyValuePair<int, string>> GetClaimNoteTypes();
        void AddOrUpdateNote(int claimId, string note, string enteredByUserId, int noteTypeId);
    }
}