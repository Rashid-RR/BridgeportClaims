using System;
using System.Collections.Generic;
using System.Linq;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Entities.DomainModels;

namespace BridgeportClaims.Data.DataProviders.Users
{
    public class UsersProvider : IUsersProvider
    {
        private readonly IRepository<AspNetUsers> _usersRepository;
        private const string Jordan = "jordan";
        private const string Gurney = "gurney";

        public UsersProvider(IRepository<AspNetUsers> usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public IEnumerable<UserDto> GetUsers() => _usersRepository.GetMany(x =>
                x.LockoutEnabled && null != x.LockoutEndDateUtc && x.LockoutEndDateUtc.Value > DateTime.UtcNow &&
                null != x.FirstName && x.FirstName.ToLower() != Jordan && null != x.LastName &&
                x.LastName.ToLower() != Gurney)
            .Select(u => new UserDto {Id = u.Id, FirstName = u.FirstName, LastName = u.LastName});
    }
}