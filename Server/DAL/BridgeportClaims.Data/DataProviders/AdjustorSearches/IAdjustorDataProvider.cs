using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.AdjustorSearches
{
    public interface IAdjustorDataProvider
    {
        IList<AdjustorSearchResultsDto> GetAdjustorSearchResults(string searchText);
        IEnumerable<AdjustorNameDto> GetAdjustorNames(string adjustorName);
        AdjustorDto GetAdjustors(string searchText, int page, int pageSize, string sort, string sortDirection);
    }
}