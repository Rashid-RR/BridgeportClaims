using System.Web.Http;
using System.Web.Http.Cors;
using BridgeportClaims.Services.Config;
using BridgeportClaims.Web.Formatters;
using BridgeportClaims.Web.Handlers;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ServiceStack.Text;

namespace BridgeportClaims.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

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
        }
    }
}
