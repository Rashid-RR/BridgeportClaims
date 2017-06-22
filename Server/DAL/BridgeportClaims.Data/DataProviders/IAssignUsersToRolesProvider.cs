using System.Collections.Generic;

namespace BridgeportClaims.Data.DataProviders
{
    public interface IAssignUsersToRolesProvider
    {
        void AssignUsersToRoles(string userName, IList<string> roles);
    }
}