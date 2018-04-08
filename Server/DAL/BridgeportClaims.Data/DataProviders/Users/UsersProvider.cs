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
        private readonly Lazy<IRepository<AspNetUsers>> _usersRepository;
        private const string Jordan = "Jordan";
        private const string Gurney = "Gurney";

        public UsersProvider(Lazy<IRepository<AspNetUsers>> usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public IEnumerable<UserDto> GetUsers() => _usersRepository.Value.GetMany(x =>
                (null == x.LockoutEndDateUtc || x.LockoutEnabled && null != x.LockoutEndDateUtc && x.LockoutEndDateUtc.Value > DateTime.UtcNow)
                && (null != x.FirstName && x.FirstName != Jordan || null != x.LastName && x.LastName != Gurney)
                ).Select(u => new UserDto {Id = u.Id, FirstName = u.FirstName, LastName = u.LastName});
    }
}