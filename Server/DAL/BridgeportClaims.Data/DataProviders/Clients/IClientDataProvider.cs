using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.Clients
{
    public interface IClientDataProvider
    {
        void InsertReferral(ReferralDto referral);
        IEnumerable<ReferralTypeDto> GetReferralTypes();
        IEnumerable<UsStateDto> GetUsStates();
        string SetUserType(string userId, int referralTypeId, string modifiedByUserId);
    }
}