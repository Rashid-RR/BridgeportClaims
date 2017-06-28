using System.Collections.Generic;
using System.Collections.Specialized;

namespace BridgeportClaims.Data.DataProviders
{
    public interface IClaimNotesDataProvider
    {
        IList<KeyValuePair<int, string>> GetClaimNoteTypes();
        void AddOrUpdateNote(int claimId, string note, string enteredByUserId, int noteTypeId);
    }
}