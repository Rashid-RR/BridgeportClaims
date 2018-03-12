using System.Collections.Generic;
using BridgeportClaims.Entities.DomainModels;
using BridgeportClaims.Entities.ViewModels;

namespace BridgeportClaims.Data.DataProviders.Payors
{
    public interface IPayorsDataProvider
    {
        IEnumerable<Payor> GetAllPayors();
        IList<PayorViewModel> GetPaginatedPayors(int pageNumber, int pageSize);
    }
}