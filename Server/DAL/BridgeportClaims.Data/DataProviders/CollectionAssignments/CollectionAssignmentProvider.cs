using Dapper;
using System.Data;
using System.Data.SqlClient;
using BridgeportClaims.Common.Disposable;
using cs = BridgeportClaims.Common.Config.ConfigService;
using s = BridgeportClaims.Common.Constants.StringConstants;


namespace BridgeportClaims.Data.DataProviders.CollectionAssignments
{
    public class CollectionAssignmentProvider : ICollectionAssignmentProvider
    {
        public void MergeCollectionAssignments(string userId, string modifiedByUserId, DataTable dt)
            => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[dbo].[uspMergeCollectionAssignments]";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                var ps = new DynamicParameters();
                ps.Add("@UserID", userId, DbType.String, size: 128);
                ps.Add("@ModifiedByUserID", modifiedByUserId, DbType.String, size: 128);
                ps.Add("@Payors", dt.AsTableValuedParameter(s.UdtId));
                conn.Execute(sp, ps, commandType: CommandType.StoredProcedure);
            });
    }
}