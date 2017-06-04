using System.Collections.Generic;
using AutoMapper;
using BridgeportClaims.Entities.DomainModels;
using BridgeportClaims.Entities.ViewModels;

namespace BridgeportClaims.Entities.Automappers
{
    public class PayorMapper : IPayorMapper
    {
        public IList<PayorViewModel> GetPayorViewModels(IList<Payor> payors) => Mapper
            .Map<IList<Payor>, IList<PayorViewModel>>(payors);

        public PayorViewModel GetPayorViewModel(Payor payor) =>
            Mapper.Map<Payor, PayorViewModel>(payor);
    }
}
