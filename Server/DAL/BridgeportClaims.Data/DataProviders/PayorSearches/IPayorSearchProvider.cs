using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.PayorSearches
{
    public interface IPayorSearchProvider
    {
        IList<PayorSearchResultsDto> GetPayorSearchResults(string searchText);
    }
}