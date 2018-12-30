using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.AttorneyProviders
{
    public interface IAttorneyProvider
    {
        AttorneyDto GetAttorneys(string searchText, int page, int pageSize, string sort, string sortDirection);
        AttorneyResultDto InsertAttorney(string attorneyName, string address1, string address2, string city,
            int stateId, string postalCode, string phoneNumber, string faxNumber, string modifiedByUserId);
        AttorneyResultDto UpdateAttorney(int modelAttorneyId, string modelAttorneyName, string modelAddress1,
            string modelAddress2, string modelCity, int modelStateId, string modelPostalCode, string modelPhoneNumber, string modelFaxNumber, string userId);
    }
}