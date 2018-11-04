namespace BridgeportClaims.Data.DataProviders.CollectionAssignments
{
    public interface ICollectionAssignmentProvider
    {
        void InsertCollectionAssignment(string userId, int payorId, string modifiedByUserId);
        void DeleteCollectionAssignment(string userId, int payorId);
        void UpdateCollectionAssignment(string userId, int payorId, string modifiedByUserId);
    }
}