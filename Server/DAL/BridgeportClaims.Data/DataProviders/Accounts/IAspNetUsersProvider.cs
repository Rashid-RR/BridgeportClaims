using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.Accounts
{
    public interface IAspNetUsersProvider
    {
        void UpdatePersonalData(string userId, string firstName, string lastName, string extension);
        IEnumerable<AspNetUsersDto> GetAllUsers();
    }
}