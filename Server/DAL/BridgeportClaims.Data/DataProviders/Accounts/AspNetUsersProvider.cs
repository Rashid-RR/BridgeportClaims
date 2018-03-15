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

        public void UpdatePersonalData(string userId, string firstName, string lastName, string extension)
        {
            var user = _usersRepository.Get(userId);
            if (null == user)
                throw new Exception($"The {nameof(user)} was not found from user Id: {userId}");
            if (firstName.IsNotNullOrWhiteSpace())
                user.FirstName = firstName;
            if (lastName.IsNotNullOrWhiteSpace())
                user.LastName = lastName;
            if (extension.IsNotNullOrWhiteSpace())
                user.Extension = extension;
            _usersRepository.Update(user);
        }
    }
}