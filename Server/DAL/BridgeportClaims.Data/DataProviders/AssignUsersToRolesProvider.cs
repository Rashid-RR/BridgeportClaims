using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BridgeportClaims.Data.StoredProcedureExecutors;

namespace BridgeportClaims.Data.DataProviders
{
    public class AssignUsersToRolesProvider : IAssignUsersToRolesProvider
    {
        private readonly IStoredProcedureExecutor _execr;

        public AssignUsersToRolesProvider(IStoredProcedureExecutor execr)
        {
            _execr = execr;
        }

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
                _execr.ExecuteNoResultStoredProcedure(
                    "EXECUTE [dbo].[uspAssignUserToRole] @Email = :Email, @RoleName = :RoleName",
                    new List<SqlParameter> {emailParameter, roleNameParameter});
            }
            
        }
    }
}