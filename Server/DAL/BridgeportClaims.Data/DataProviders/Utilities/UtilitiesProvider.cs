using System.Data;
using System.Data.SqlClient;
using cs = BridgeportClaims.Common.Config.ConfigService;
using BridgeportClaims.Common.Disposable;

namespace BridgeportClaims.Data.DataProviders.Utilities
{
    public class UtilitiesProvider : IUtilitiesProvider
    {
        public int ReseedTableAndGetSeedValue(string tableName)
            => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                return DisposableService.Using(() => new SqlCommand("dbo.uspReseedTableWithSeedValue", conn), cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var tableNameParm = cmd.CreateParameter();
                    tableNameParm.Direction = ParameterDirection.Input;
                    tableNameParm.Value = tableName;
                    tableNameParm.ParameterName = "@TableName";
                    tableNameParm.DbType = DbType.AnsiStringFixedLength;
                    tableNameParm.SqlDbType = SqlDbType.NVarChar;
                    tableNameParm.Size = 128;
                    cmd.Parameters.Add(tableNameParm);
                    var seedValueParam = cmd.CreateParameter();
                    seedValueParam.Direction = ParameterDirection.Output;
                    seedValueParam.DbType = DbType.Int32;
                    seedValueParam.SqlDbType = SqlDbType.Int;
                    seedValueParam.ParameterName = "@SeedValue";
                    cmd.Parameters.Add(seedValueParam);
                    if (ConnectionState.Open != conn.State)
                        conn.Open();
                    cmd.ExecuteNonQuery();
                    var retVal = seedValueParam.Value as int? ?? default;
                    return retVal;
                });
            });
    }
}