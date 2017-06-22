using System.Collections.Generic;
using BridgeportClaims.Entities.DomainModels;
using BridgeportClaims.Entities.ViewModels;

namespace BridgeportClaims.Entities.Automappers
{
    public interface IPayorMapper
    {
        IList<PayorViewModel> GetPayorViewModels(IList<Payor> payors);
        PayorViewModel GetPayorViewModel(Payor payor);
    }
}