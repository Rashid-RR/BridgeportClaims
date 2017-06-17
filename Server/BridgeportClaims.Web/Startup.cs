using System.Web.Http;
using System.Web.Http.Cors;
using BridgeportClaims.Services.Config;
using BridgeportClaims.Web.Formatters;
using BridgeportClaims.Web.Handlers;
using BridgeportClaims.Web.Models;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using ServiceStack.Text;

[assembly: OwinStartup(typeof(BridgeportClaims.Web.Startup))]

namespace BridgeportClaims.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var httpConfig = new HttpConfiguration();
            ConfigureOAuthTokenGeneration(app);
            ConfigureWebApi(httpConfig);
            // app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(httpConfig);
        }

        private void ConfigureOAuthTokenGeneration(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            // Plugin the OAuth bearer JSON Web Token tokens generation and Consumption will be here
        }

        private void ConfigureWebApi(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Formatters.RemoveAt(0);
            config.Formatters.Insert(0, new ServiceStackTextFormatter());
            JsConfig.EmitCamelCaseNames = true;
            config.MessageHandlers.Add(new EncodingDelegateHandler());

            var corsHostName = new ConfigService();

            // Enable CORS
            var cors = new EnableCorsAttribute(corsHostName.GetConfigItem("AllowCorsHostName"), "*", "*");
            config.EnableCors(cors);

            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            //var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            //jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
