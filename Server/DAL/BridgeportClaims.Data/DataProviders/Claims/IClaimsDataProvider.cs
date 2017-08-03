using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.Claims
{
    public interface IClaimsDataProvider
    {
        IList<GetClaimsSearchResults> GetClaimsData(string claimNumber, string firstName, string lastName,
            string rxNumber, string invoiceNumber);

        ClaimDto GetClaimsDataByClaimId(int claimId);
    }
}