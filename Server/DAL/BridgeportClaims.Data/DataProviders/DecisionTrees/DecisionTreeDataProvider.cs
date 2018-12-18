using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using Dapper;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.DecisionTrees
{
    public class DecisionTreeDataProvider : IDecisionTreeDataProvider
    {
        public DecisionTreeDto InsertDecisionTree(int parentTreeId, string nodeName,
            string nodeDescription, string modifiedByUserId) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                const string sp = "[dbo].[uspDecisionTreeInsert]";
                var ps = new DynamicParameters();
                ps.Add("@ParentTreeID", parentTreeId, DbType.Int32);
                ps.Add("@NodeName", nodeName, DbType.AnsiString, size: 255);
                ps.Add("@NodeDescription", nodeDescription, DbType.AnsiString, size: 4000);
                ps.Add("@ModifiedByUserID", modifiedByUserId, DbType.String, size: 128);
                var decisionTree = conn.Query<DecisionTreeDto>(sp, ps, commandType: CommandType.StoredProcedure);
                return decisionTree?.SingleOrDefault();
            });

        public IEnumerable<DecisionTreeDto> GetDecisionTree(int parentTreeId) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                const string sp = "[dbo].[uspGetDecisionTree]";
                return conn.Query<DecisionTreeDto>(sp, new {ParentTreeID = parentTreeId},
                    commandType: CommandType.StoredProcedure);
            });
    }
}