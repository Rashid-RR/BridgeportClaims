using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.Payors
{
    public interface IPayorsDataProvider
    {
        IEnumerable<PayorDto> GetPayors();
        IEnumerable<PayorFullDto> GetAllPayors();
        IList<PayorFullDto> GetPaginatedPayors(int pageNumber, int pageSize);
        IEnumerable<PayorDto> GetPayors(string userId);
        PayorListDto GetPayorList(string searchText, int page, int pageSize, string sort, string sortDirection);
    }
}