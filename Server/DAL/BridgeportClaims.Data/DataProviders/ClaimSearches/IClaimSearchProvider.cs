using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.ClaimSearches
{
    public interface IClaimSearchProvider
    {
        IEnumerable<ClaimResultDto> GetSearchClaimResults(string searchTerm, SearchType searchType);
        IList<DocumentClaimSearchResultDto> GetDocumentClaimSearchResults(string searchText, bool exactMatch, string delimiter);
    }
}