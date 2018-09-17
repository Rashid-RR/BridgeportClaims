using System.Data;
using System.Data.SqlClient;
using cs = BridgeportClaims.Maintenance.Business.ConfigService.ConfigService;
using BridgeportClaims.Maintenance.Business.Disposable;
using Dapper;

namespace BridgeportClaims.Maintenance.Business.DAL
{
    public static class MaintenanceService
    {
        public static void ExecuteMaintenance() =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "util.uspMaintenance";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                conn.Execute(sp, commandType: CommandType.StoredProcedure);
            });
    }
}
