using System.Data;
using System.Data.SqlClient;
using BridgeportClaims.Common.Disposable;
using Dapper;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.CollectionAssignments
{
    public class CollectionAssignmentProvider : ICollectionAssignmentProvider
    {
        public void InsertCollectionAssignment(string userId, int payorId, string modifiedByUserId)
            => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[dbo].[uspCollectionAssignmentInsert]";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                conn.Execute(sp, new {UserID = userId, PayorID = payorId, ModifiedByUserID = modifiedByUserId},
                    commandType: CommandType.StoredProcedure);
            });

        public void DeleteCollectionAssignment(string userId, int payorId) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[dbo].[uspCollectionAssignmentDelete]";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                conn.Execute(sp, new {UserID = userId, PayorID = payorId}, commandType: CommandType.StoredProcedure);
            });

        public void UpdateCollectionAssignment(string userId, int payorId, string modifiedByUserId) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[dbo].[uspCollectionAssignmentUpdate]";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                conn.Execute(sp, new {UserID = userId, PayorID = payorId, ModifiedByUserID = modifiedByUserId},
                    commandType: CommandType.StoredProcedure);
            });
    }
}