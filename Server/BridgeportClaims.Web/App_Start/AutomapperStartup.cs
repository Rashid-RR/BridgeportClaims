using AutoMapper;
using BridgeportClaims.Entities.DomainModels;
using BridgeportClaims.Entities.ViewModels;

namespace BridgeportClaims.Web
{
    public class AutomapperStartup
    {
        public static void Configure()
        {
            // var config = new MapperConfiguration(cfg => cfg.CreateMap<Payor, PayorViewModel>());
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Payor, PayorViewModel>()
                    .ForMember(dest => dest.State,
                        x => x.MapFrom(src => src.UsState.StateCode));
            });
        }
    }
}