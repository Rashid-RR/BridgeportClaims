using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.AdjustorSearches
{
    public interface IAdjustorDataProvider
    {
        IList<AdjustorSearchResultsDto> GetAdjustorSearchResults(string searchText);
        IEnumerable<AdjustorNameDto> GetAdjustorNames(string adjustorName);
        AdjustorDto GetAdjustors(string searchText, int page, int pageSize, string sort, string sortDirection);
        AdjustorResultDto InsertAdjustor(string adjustorName, string address1, string address2,
            string city, int? stateId, string postalCode, string phoneNumber,
            string faxNumber, string emailAddress, string extension, string modifiedByUserId);
        AdjustorResultDto UpdateAdjustor(int adjustorId, string adjustorName, string address1,
            string address2, string city, int? stateId, string postalCode, string phoneNumber,
            string faxNumber, string emailAddress, string extension, string modifiedByUserId);
    }
}