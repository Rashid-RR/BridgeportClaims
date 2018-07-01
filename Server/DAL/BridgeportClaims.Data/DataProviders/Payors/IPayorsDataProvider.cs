using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Entities.DomainModels;
using BridgeportClaims.Entities.ViewModels;

namespace BridgeportClaims.Data.DataProviders.Payors
{
    public interface IPayorsDataProvider
    {
        IEnumerable<PayorDto> GetPayors();
        IEnumerable<Payor> GetAllPayors();
        IList<PayorViewModel> GetPaginatedPayors(int pageNumber, int pageSize);
    }
}