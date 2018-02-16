using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.Users
{
    public interface IUsersProvider
    {
        IEnumerable<UserDto> GetUsers();
    }
}