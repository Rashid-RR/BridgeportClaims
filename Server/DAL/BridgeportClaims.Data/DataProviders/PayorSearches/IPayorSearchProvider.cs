using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.PayorSearches
{
    public interface IPayorSearchProvider
    {
        IEnumerable<PayorSearchResultsDto> GetPayorSearchResults(string searchText);
    }
}