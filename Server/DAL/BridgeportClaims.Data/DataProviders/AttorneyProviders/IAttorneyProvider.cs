using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.AttorneyProviders
{
    public interface IAttorneyProvider
    {
        AttorneyDto GetAttorneys(string searchText, int page, int pageSize, string sort, string sortDirection);
        AttorneyResultDto InsertAttorney(string attorneyName, string address1, string address2, string city,
            int? stateId, string postalCode, string phoneNumber, string faxNumber, string modifiedByUserId);
        AttorneyResultDto UpdateAttorney(int attorneyId, string attorneyName, string address1,
            string address2, string city, int? stateId, string postalCode, string phoneNumber, string faxNumber, string userId);
    }
}