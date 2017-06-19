using System;
using System.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;
using BridgeportClaims.Services.Config;
using BridgeportClaims.Web.Formatters;
using BridgeportClaims.Web.Handlers;
using BridgeportClaims.Web.Models;
using BridgeportClaims.Web.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Owin;
using ServiceStack.Text;

[assembly: OwinStartup(typeof(BridgeportClaims.Web.Startup))]

namespace BridgeportClaims.Web
{
    public partial class Startup
    {
        private const string Issuer = "http://bridgeportclaims.com";

        public void Configuration(IAppBuilder app)
        {
            var httpConfig = new HttpConfiguration();
            ConfigureOAuthTokenGeneration(app);
            ConfigureOAuthTokenConsumption(app);
            ConfigureWebApi(httpConfig);
            // app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(httpConfig);
        }

        private void ConfigureOAuthTokenConsumption(IAppBuilder app)
        {

            const string issuer = Issuer;
            var audienceId = ConfigurationManager.AppSettings["as:AudienceId"];
            var audienceSecret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["as:AudienceSecret"]);

            // Api controllers with an [Authorize] attribute will be validated with JWT
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { audienceId },
                    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {
                        new SymmetricKeyIssuerSecurityTokenProvider(issuer, audienceSecret)
                    }
                });
        }

        private void ConfigureOAuthTokenGeneration(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            var oAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                //For Dev enviroment only (on production should be AllowInsecureHttp = false)
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/oauth/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new CustomOAuthProvider(),
                AccessTokenFormat = new CustomJwtFormat(Issuer)
            };

            // OAuth 2.0 Bearer Access Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
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
