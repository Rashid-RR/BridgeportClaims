using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BridgeportClaims.Common.Disposable;
using Dapper;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.UserRoles
{
    public class AssignUsersToRolesProvider : IAssignUsersToRolesProvider
    {
        public void AssignUserToRole(string userId, string roleId) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                DisposableService.Using(() => new SqlCommand("dbo.uspAddUserToRole", conn), cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var userIdParam = cmd.CreateParameter();
                    userIdParam.Direction = ParameterDirection.Input;
                    userIdParam.Value = userId ?? (object) DBNull.Value;
                    userIdParam.ParameterName = "@UserID";
                    userIdParam.DbType = DbType.String;
                    userIdParam.SqlDbType = SqlDbType.NVarChar;
                    userIdParam.Size = 128;
                    cmd.Parameters.Add(userIdParam);
                    var roleIdParam = cmd.CreateParameter();
                    roleIdParam.Direction = ParameterDirection.Input;
                    roleIdParam.DbType = DbType.String;
                    roleIdParam.Size = 128;
                    roleIdParam.Value = roleId ?? (object) DBNull.Value;
                    roleIdParam.ParameterName = "@RoleID";
                    roleIdParam.SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.Add(roleIdParam);
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    cmd.ExecuteNonQuery();
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                });
            });

        public void AssignUsersToRoles(string userName, IEnumerable<string> roles)
        {
            foreach (var role in roles)
            {
                DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
                {
                    const string sp = "[dbo].[uspAssignUserToRole]";
                    var ps = new DynamicParameters();
                    ps.Add("@Email", userName, DbType.String, ParameterDirection.Input, 256);
                    ps.Add("@RoleName", role, DbType.String, ParameterDirection.Input, 256);
                    conn.Open();
                    conn.Execute(sp, ps, commandType: CommandType.StoredProcedure);
                });
            }
        }
    }
}