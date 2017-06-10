using System.Collections.Generic;
using BridgeportClaims.Data.StoredProcedureExecutors.Dtos;

namespace BridgeportClaims.Data.DataProviders
{
    public interface IGetClaimsDataProvider
    {
        IList<GetClaimsData> GetClaimsData(string claimNumber, string firstName, string lastName,
            string rxNumber, string invoiceNumber);
    }
}