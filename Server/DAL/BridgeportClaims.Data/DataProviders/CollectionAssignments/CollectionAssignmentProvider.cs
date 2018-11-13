using System.Collections.Generic;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
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

        public CollectionAssignmentData GetCollectionAssignmentData(string userId) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[dbo].[uspGetCollectionAssignmentData]";
                var retVal = new CollectionAssignmentData();
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                var ps = new DynamicParameters();
                ps.Add("@UserID", userId, DbType.String, size: 128);
                var multi = conn.QueryMultiple(sp, ps, commandType: CommandType.StoredProcedure);
                var left = multi.Read<PayorDto>();
                var right = multi.Read<PayorDto>();
                retVal.LeftCarriers = left?.OrderBy(o => o.Carrier).ToList() ?? new List<PayorDto>();
                retVal.RightCarriers = right?.OrderBy(o => o.Carrier).ToList() ?? new List<PayorDto>();
                return retVal;
            });
    }
}