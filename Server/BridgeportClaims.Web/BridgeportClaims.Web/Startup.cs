using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(BridgeportClaims.Web.Startup))]

namespace BridgeportClaims.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
