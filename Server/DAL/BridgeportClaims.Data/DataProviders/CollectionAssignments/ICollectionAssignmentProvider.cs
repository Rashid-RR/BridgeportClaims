using System.Data;

namespace BridgeportClaims.Data.DataProviders.CollectionAssignments
{
    public interface ICollectionAssignmentProvider
    {
        void MergeCollectionAssignments(string userId, string modifiedByUserId, DataTable dt);
    }
}