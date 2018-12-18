using System.Data;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.CollectionAssignments
{
    public interface ICollectionAssignmentProvider
    {
        void MergeCollectionAssignments(string userId, string modifiedByUserId, DataTable dt);
        CollectionAssignmentData GetCollectionAssignmentData(string userId);
    }
}