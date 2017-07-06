using System.Collections.Generic;

namespace BridgeportClaims.Data.DataProviders.UserRoles
{
    public interface IAssignUsersToRolesProvider
    {
        void AssignUsersToRoles(string userName, IList<string> roles);
    }
}