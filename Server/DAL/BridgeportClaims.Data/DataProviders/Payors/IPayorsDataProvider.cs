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
        PayorResultDto PayorInsert(string groupName, string billToName, string billToAddress1,
            string billToAddress2, string billToCity, int? billToStateId, string billToPostalCode, string phoneNumber,
            string alternatePhoneNumber, string faxNumber, string notes, string contact, string letterName, string modifiedByUserId);
        PayorResultDto PayorUpdate(int payorId, string groupName, string billToName, string billToAddress1,
            string billToAddress2, string billToCity, int? billToStateId, string billToPostalCode, string phoneNumber,
            string alternatePhoneNumber, string faxNumber, string notes, string contact, string letterName, string modifiedByUserId);
    }
}