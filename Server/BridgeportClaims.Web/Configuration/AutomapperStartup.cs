using AutoMapper;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Web.Framework.Models;

namespace BridgeportClaims.Web.Configuration
{
    public class AutomapperStartup
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
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