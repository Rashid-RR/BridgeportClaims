using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders
{
    public interface IGetClaimsDataProvider
    {
        IList<GetClaimsSearchResults> GetClaimsData(string claimNumber, string firstName, string lastName,
            string rxNumber, string invoiceNumber);

        dynamic GetClaimsDataByClaimId(int claimId);
    }
}