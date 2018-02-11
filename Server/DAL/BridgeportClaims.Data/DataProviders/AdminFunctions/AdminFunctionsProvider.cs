using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.AdminFunctions
{
    public class AdminFunctionsProvider : IAdminFunctionsProvider
    {
        private const string Query = @"SELECT r.[name] RuleName
                                            , r.[start_ip_address] StartIpAddress
                                            , r.[end_ip_address] EndIpAddress
                                       FROM sys.firewall_rules AS r";

        public IList<FirewallSetting> GetFirewallSettings() =>
            DisposableService.Using(() => new SqlConnection(cs.GetSecureDbConnStr()), conn =>
            {
                return DisposableService.Using(() => new SqlCommand(Query, conn), cmd =>
                {
                    cmd.CommandType = CommandType.Text;
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    return DisposableService.Using(cmd.ExecuteReader, reader =>
                    {
                        var ruleNameOrdinal = reader.GetOrdinal("RuleName");
                        var startIpAddress = reader.GetOrdinal("StartIpAddress");
                        var endIpAddress = reader.GetOrdinal("EndIpAddress");
                        IList<FirewallSetting> retVal = new List<FirewallSetting>();
                        while (reader.Read())
                        {
                            var result = new FirewallSetting
                            {
                                RuleName = !reader.IsDBNull(ruleNameOrdinal) ? reader.GetString(ruleNameOrdinal) : string.Empty,
                                StartIpAddress = !reader.IsDBNull(startIpAddress) ? reader.GetString(startIpAddress) : string.Empty,
                                EndIpAddress = !reader.IsDBNull(endIpAddress) ? reader.GetString(endIpAddress) : string.Empty
                            };
                            retVal.Add(result);
                        }
                        return retVal;
                    });
                });
            });
           
    }
}