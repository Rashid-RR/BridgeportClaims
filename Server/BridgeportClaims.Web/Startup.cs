using Owin;
using System;
using System.Web.Http;
using System.Web.UI.WebControls;
using Autofac;
using Autofac.Integration.WebApi;
using BridgeportClaims.Web.Configuration;
using BridgeportClaims.Web.Framework.Formatters;
using BridgeportClaims.Web.Framework.Handlers;
using BridgeportClaims.Web.Framework.Infrastructure;
using BridgeportClaims.Web.Framework.IoCConfig;
using BridgeportClaims.Web.Framework.Middleware;
using BridgeportClaims.Web.Framework.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using NLog;
using ServiceStack.Text;
using aa = System.Web.Http.AuthorizeAttribute;
using cs = BridgeportClaims.Common.Config.ConfigService;
using am = Microsoft.Owin.Security.AuthenticationMode;

namespace BridgeportClaims.Web
{
    /// <summary>
    ///  Oh hell yeah this class is used.
    /// </summary>
    public class Startup
    {
        internal static string PublicClientId => "LOCAL AUTHORITY";
        public void Configuration(IAppBuilder app)
        {
            // Add SignalR to the OWIN pipeline
            var config = new HttpConfiguration();
            config.Formatters.Add(new BinaryMediaTypeFormatter());
            app.MapSignalR();
            var builder = IoCConfigService.Configure();
            var container = builder.Build();
            var logger = container.Resolve<ILogger>();
            if (cs.AppIsInDebugMode)
            {
                logger.Info("The Autofac container is resolving instances of ILogger.");
                logger.Info("Application Started.");
            }
            app.UseAutofacMiddleware(container);
            var resolver = new AutofacWebApiDependencyResolver(container);
            config.DependencyResolver = resolver;

            app.Use<BridgeportClaimsMiddleware>();
            config.MessageHandlers.Add(new CancelledTaskBugWorkaroundMessageHandler());
            ConfigureOAuthTokenGeneration(app);
            ConfigureOAuthTokenConsumption(app);
            ConfigureWebApi(config);
            app.UseCors(CorsOptions.AllowAll);
            AutomapperStartup.Configure();
            app.UseWebApi(config);
            app.UseAutofacWebApi(config);
        }

        private static void ConfigureOAuthTokenConsumption(IAppBuilder app)
        {
            var issuer = PublicClientId;
            var audienceId = cs.GetAppSetting("AudienceId");
            var audienceSecret = TextEncodings.Base64Url.Decode(cs.GetAppSetting("AudienceSecret"));

            // Api controllers with an [Authorize] attribute will be validated with JWT
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = am.Active,
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
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
            const string forceSsl = "forceHttps";
            // Plugin the OAuth bearer JSON Web Token tokens generation and Consumption will be here
            var oAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                // For Dev environment only (on production should be AllowInsecureHttp = false)
                AllowInsecureHttp = !Convert.ToBoolean(cs.GetAppSetting(forceSsl)),
                TokenEndpointPath = new PathString("/oauth/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(7),
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
                "DefaultApi",
                "api/{controller}/{action}/{id}",
                new { id = System.Web.Http.RouteParameter.Optional }
            );
            config.Filters.Add(new aa());
            config.Formatters.RemoveAt(0);
            config.Formatters.Insert(0, new ServiceStackTextFormatter());
            JsConfig.EmitCamelCaseNames = true;
            JsConfig.IncludeNullValues = true;

            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
        }
    }
}