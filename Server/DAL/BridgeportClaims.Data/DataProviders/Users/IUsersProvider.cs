using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.Users
{
    public interface IUsersProvider
    {
        UserDto GetUser(string userId);
        IEnumerable<UserDto> GetUsers();
    }
}