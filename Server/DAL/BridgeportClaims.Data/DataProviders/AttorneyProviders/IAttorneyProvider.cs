using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.AttorneyProviders
{
    public interface IAttorneyProvider
    {
        AttorneyDto GetAttorneys(string searchText, int page, int pageSize, string sort, string sortDirection);
    }
}