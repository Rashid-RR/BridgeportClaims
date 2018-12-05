using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using s = SPOT.Admin.Code.Constants.StringConstants;

namespace SPOT.Admin.Code.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAppWeb(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<EnvConfig>(config.GetSection(s.EnvironmentConfigSection));

            // IoC
        }
    }
}