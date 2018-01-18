using System;

namespace BridgeportClaims.Data.DataProviders.ClaimsEdit
{
    public interface IClaimsEditProvider
    {
        void EditClaim(int claimId, string modifiedByUserId, DateTime? dateOfBirth, int genderId, int payorId, int? adjustorId, string adjustorPhone,
            DateTime? dateOfInjury, string adjustorFax);
    }
}