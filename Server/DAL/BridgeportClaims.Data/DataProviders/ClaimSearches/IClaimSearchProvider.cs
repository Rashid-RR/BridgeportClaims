using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.ClaimSearches
{
    public interface IClaimSearchProvider
    {
        IList<DocumentClaimSearchResultDto> GetDocumentClaimSearchResults(string searchText, bool exactMatch, char delimiter);
    }
}