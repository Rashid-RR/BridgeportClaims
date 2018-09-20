using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Common.Disposable;
using Dapper;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.Repositories
{
    public class DapperRepository
    {
        private const int Timeout = 1800;

        protected IEnumerable<TModel> ExecuteAndReturnCollection<TModel>(string sp, DynamicParameters parameters) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<TModel>(sp, parameters, commandType: CommandType.StoredProcedure,
                    commandTimeout: Timeout);
            });

        protected TModel ExecuteAndReturnSingleOrDefault<TModel>(string sp, DynamicParameters parameters) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<TModel>(sp,
                    parameters, commandType: CommandType.StoredProcedure,
                    commandTimeout: Timeout).SingleOrDefault();
            });

        protected void ExecuteVoid(string sp, DynamicParameters parameters) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                conn.Execute(sp, parameters, commandType: CommandType.StoredProcedure, commandTimeout: Timeout);
            });
    }
}