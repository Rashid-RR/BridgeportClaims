using AutoMapper;
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
                        x => x.MapFrom(src => src.UsState.StateCode));
                cfg.CreateMap<UserPaymentPostingSession, PaymentPostingViewModel>();
            });
        }
    }
}