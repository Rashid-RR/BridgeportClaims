namespace BridgeportClaims.Data.DataProviders
{
    public interface IClaimNotesDataProvider
    {
        void AddOrUpdateNote(int claimId, string note, string enteredByUserId, int noteTypeId);
    }
}