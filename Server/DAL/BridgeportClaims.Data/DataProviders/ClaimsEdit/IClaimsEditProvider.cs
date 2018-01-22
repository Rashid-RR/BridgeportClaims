using System;

namespace BridgeportClaims.Data.DataProviders.ClaimsEdit
{
    public interface IClaimsEditProvider
    {
        void EditClaim(int claimId, string modifiedByUserId, DateTime? dateOfBirth, int genderId, int payorId, int? adjustorId, string adjustorPhone,
            DateTime? dateOfInjury, string adjustorFax, string address1, string address2, string city, int? stateId, string postalCode, int? claimFlex2Id);
    }
}