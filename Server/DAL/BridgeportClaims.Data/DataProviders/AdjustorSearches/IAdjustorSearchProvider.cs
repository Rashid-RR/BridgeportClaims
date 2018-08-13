using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.AdjustorSearches
{
    public interface IAdjustorSearchProvider
    {
        IList<AdjustorSearchResultsDto> GetAdjustorSearchResults(string searchText);
        IEnumerable<AdjustorNameDto> GetAdjustorNames(string adjustorName);
    }
}