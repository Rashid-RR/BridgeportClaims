using AutoMapper;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Entities.DomainModels;
using BridgeportClaims.Entities.ViewModels;
using BridgeportClaims.Web.Models;

namespace BridgeportClaims.Web
{
    public class AutomapperStartup
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Payor, PayorViewModel>()
                    .ForMember(dest => dest.State,
                        x => x.MapFrom(src => src.BillToStateId.StateCode));
                cfg.CreateMap<UserPaymentPostingSession, PaymentPostingViewModel>();
                cfg.CreateMap<PaymentPosting, PaymentPostingDto>()
                    .ForMember(dest => dest.PrescriptionID,
                        x => x.MapFrom(src => src.PrescriptionId))
                    .ForMember(dest => dest.AmountPosted,
                        x => x.MapFrom(src => src.AmountPosted));
            });
        }
    }
}