using System;
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
        public Guid DecisionTreeHeaderInsert(string userId, int treeRootId, int claimId)
            => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()),
            conn =>
            {
                const string sp = "[dbo].[uspDecisionTreeUserPathHeaderInsert]";
                const string sessionIdParam = "@SessionID";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                var ps = new DynamicParameters();
                ps.Add("@UserID", userId, DbType.String, size: 128);
                ps.Add("@TreeRootID", treeRootId, DbType.Int32);
                ps.Add("@ClaimID", claimId, DbType.Int32);
                ps.Add(sessionIdParam, DbType.Guid, direction: ParameterDirection.Output);
                conn.Execute(sp, ps, commandType: CommandType.StoredProcedure);
                return ps.Get<Guid>(sessionIdParam);
            });

        public DecisionTreeDto InsertDecisionTree(int parentTreeId, string nodeName,
            string modifiedByUserId) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                const string sp = "[dbo].[uspDecisionTreeInsert]";
                const string rootTreeId = "@RootTreeID";
                var ps = new DynamicParameters();
                ps.Add("@ParentTreeID", parentTreeId, DbType.Int32);
                ps.Add("@NodeName", nodeName, DbType.AnsiString, size: 255);
                ps.Add("@ModifiedByUserID", modifiedByUserId, DbType.String, size: 128);
                ps.Add(rootTreeId, dbType: DbType.Int32, direction: ParameterDirection.Output);
                return conn.Query<DecisionTreeDto>(sp, ps, commandType: CommandType.StoredProcedure)
                    ?.SingleOrDefault();
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

        public DecisionTreeListDto GetDecisionTreeList(string searchText, string sort, string sortDirection, int page,
            int pageSize) => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
        {
            const string output = "@totalRows";
            const string sp = "[dbo].[uspGetDecisionTreeList]";
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            var ps = new DynamicParameters();
            ps.Add("@SearchText", searchText, DbType.AnsiString, size: 4000);
            ps.Add("@SortColumn", sort, DbType.AnsiString, size: 50);
            ps.Add("@SortDirection", sortDirection, DbType.AnsiString, size: 5);
            ps.Add("@PageNumber", page, DbType.Int32);
            ps.Add("@PageSize", pageSize, DbType.Int32);
            ps.Add(output, dbType: DbType.Int32, direction: ParameterDirection.Output);
            var results = conn.Query<DecisionTreeListResultDto>(sp, ps, commandType: CommandType.StoredProcedure);
            var rows = ps.Get<int>(output);
            var retVal = new DecisionTreeListDto
            {
                Results = results,
                TotalRows = rows
            };
            return retVal;
        });

        public int DeleteDecisionTree(int treeId)
            => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                const string sp = "[dbo].[uspDecisionTreeDeleteNode]";
                const string rowCount = "@RowCount";
                var ps = new DynamicParameters();
                ps.Add("@TreeID", treeId, DbType.Int32, ParameterDirection.Input);
                ps.Add(rowCount, dbType: DbType.Int32, direction: ParameterDirection.Output);
                conn.Execute(sp, ps, commandType: CommandType.StoredProcedure);
                return ps.Get<int>(rowCount);
            });
    }
}