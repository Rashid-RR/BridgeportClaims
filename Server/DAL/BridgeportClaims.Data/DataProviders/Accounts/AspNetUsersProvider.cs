using System;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Entities.DomainModels;
using BridgeportClaims.Common.Extensions;

namespace BridgeportClaims.Data.DataProviders.Accounts
{
    public class AspNetUsersProvider : IAspNetUsersProvider
    {
        private readonly IRepository<AspNetUsers> _usersRepository;

        public AspNetUsersProvider(IRepository<AspNetUsers> usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public void UpdateFirstOrLastName(string userId, string firstName, string lastName)
        {
            var user = _usersRepository.Get(userId);
            if (null == user)
                throw new Exception($"The {nameof(user)} was not found from user Id: {userId}");
            if (firstName.IsNotNullOrWhiteSpace())
                user.FirstName = firstName;
            if (lastName.IsNotNullOrWhiteSpace())
                user.LastName = lastName;
            _usersRepository.Update(user);
        }

        public void DeactivateUser(string userId)
        {
            var user = _usersRepository.Get(userId);
            if (null == user)
                throw new Exception($"The {nameof(user)} was not found from user Id: {userId}");
            if (!user.LockoutEnabled)
                user.LockoutEnabled = true;
            user.LockoutEndDateUtc = DateTime.UtcNow.AddYears(200);
            _usersRepository.Update(user);
        }

        public void ActivateUser(string userId)
        {
            var user = _usersRepository.Get(userId);
            if (null == user)
                throw new Exception($"The {nameof(user)} was not found from user Id: {userId}");
            user.LockoutEndDateUtc = null;
            _usersRepository.Update(user);
        }
    }
}