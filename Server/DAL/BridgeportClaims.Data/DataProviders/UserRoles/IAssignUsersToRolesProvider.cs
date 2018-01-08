using System.Collections.Generic;

namespace BridgeportClaims.Data.DataProviders.UserRoles
{
    public interface IAssignUsersToRolesProvider
    {
        void AssignUserToRole(string userId, string roleId);
        void AssignUsersToRoles(string userName, IList<string> roles);
    }
}