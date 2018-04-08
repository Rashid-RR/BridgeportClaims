using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.SessionFactory.StoredProcedureExecutors;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.UserRoles
{
    public class AssignUsersToRolesProvider : IAssignUsersToRolesProvider
    {
        private readonly Lazy<IStoredProcedureExecutor> _execr;

        public AssignUsersToRolesProvider(Lazy<IStoredProcedureExecutor> execr)
        {
            _execr = execr;
        }

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

        public void AssignUsersToRoles(string userName, IList<string> roles)
        {
            var emailParameter = new SqlParameter
            {
                ParameterName = "Email",
                Value = userName,
                DbType = DbType.String
            };
            foreach (var role in roles)
            {
                var roleNameParameter = new SqlParameter
                {
                    ParameterName = "RoleName",
                    Value = role,
                    DbType = DbType.String
                };
                _execr.Value.ExecuteNoResultStoredProcedure(
                    "EXECUTE [dbo].[uspAssignUserToRole] @Email = :Email, @RoleName = :RoleName",
                    new List<SqlParameter> {emailParameter, roleNameParameter});
            }
            
        }
    }
}