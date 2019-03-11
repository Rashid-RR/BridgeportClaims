using System;

namespace BridgeportClaims.Data.DataProviders.ClaimsEdit
{
    public interface IClaimsEditProvider
    {
        void EditClaim(int claimId, string modifiedByUserId, DateTime? ofBirth, int genderId, int payorId, int? adjustorId, int? attorneyId,
            DateTime? ofInjury, string address1, string address2, string city, int? stateId, string postalCode, int? claimFlex2Id);
    }
}