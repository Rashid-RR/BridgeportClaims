using Owin;
using System;
using Autofac;
using System.Linq;
using System.Web.Http;
using System.Net.Http.Formatting;
using Autofac.Integration.WebApi;
using BridgeportClaims.Common.Config;
using BridgeportClaims.Web.Formatters;
using BridgeportClaims.Web.Infrastructure;
using BridgeportClaims.Web.IoCConfig;
using BridgeportClaims.Web.Middleware;
using BridgeportClaims.Web.Providers;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using ServiceStack.Text;

namespace BridgeportClaims.Web
{
    public class Startup
    {
        internal static string PublicClientId => "LOCAL AUTHORITY";
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            var builder = IoCConfigService.Configure();
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            // app.UseAutofacMiddleware(container);
            ConfigureOAuthTokenGeneration(app);
            ConfigureOAuthTokenConsumption(app);
            ConfigureWebApi(config);
            app.UseCors(CorsOptions.AllowAll);
            AutomapperStartup.Configure();
            if (ConfigService.AppIsInDebugMode)
                NHibernateProfiler.Initialize();
            app.Use<BridgeportClaimsMiddleware>().UseWebApi(config);
        }
        
        private static void ConfigureOAuthTokenConsumption(IAppBuilder app)
        {
            var issuer = PublicClientId;
            var audienceId = ConfigService.GetAppSetting("as:AudienceId");
            var audienceSecret = TextEncodings.Base64Url.Decode(ConfigService.GetAppSetting("as:AudienceSecret"));

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

        private static void ConfigureOAuthTokenGeneration(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            // Plugin the OAuth bearer JSON Web Token tokens generation and Consumption will be here
            var oAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                //For Dev enviroment only (on production should be AllowInsecureHttp = false)
                AllowInsecureHttp = !Convert.ToBoolean(ConfigService.GetAppSetting("forceHttps")),
                TokenEndpointPath = new PathString("/oauth/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                Provider = new BridgeportClaimOAuthProvider(),
                AccessTokenFormat = new BridgeportClaimsJwtFormat(PublicClientId)
            };

            // OAuth 2.0 Bearer Access Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
        }

        private static void ConfigureWebApi(HttpConfiguration config)
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

            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
        }
    }
}