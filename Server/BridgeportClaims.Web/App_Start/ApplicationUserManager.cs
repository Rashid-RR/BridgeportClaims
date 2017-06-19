//using System;
//using Owin;
//using BridgeportClaims.Services.Config;
//using Microsoft.AspNet.Identity;
//using Microsoft.Owin;
//using Microsoft.Owin.Security.Cookies;
//using Microsoft.Owin.Security.OAuth;
//using BridgeportClaims.Web.Providers;
//using BridgeportClaims.Web.Models;

//namespace BridgeportClaims.Web
//{
//    public partial class Startup
//    {
//        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

//        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
//        public void ConfigureAuth(IAppBuilder app)
//        {
//            // Configure the db context and user manager to use a single instance per request
//            app.CreatePerOwinContext(ApplicationDbContext.Create);
//            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

//            // Enable the application to use a cookie to store information for the signed in user
//            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
//            app.UseCookieAuthentication(new CookieAuthenticationOptions());
//            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
//            var configService = new ConfigService();
//            // Configure the application for OAuth based flow
//            OAuthOptions = new OAuthAuthorizationServerOptions
//            {
//                TokenEndpointPath = new PathString("/Token"),
//                Provider = new ApplicationOAuthProvider(PublicClientId),
//                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
//                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
//                // In production mode set AllowInsecureHttp = false
//                AllowInsecureHttp = !Convert.ToBoolean(configService.GetConfigItem("forceHttps"))
//            };

//            // Enable the application to use bearer tokens to authenticate users
//            app.UseOAuthBearerTokens(OAuthOptions);

//            // Uncomment the following lines to enable logging in with third party login providers
//            //app.UseMicrosoftAccountAuthentication(
//            //    clientId: "",
//            //    clientSecret: "");

//            //app.UseTwitterAuthentication(
//            //    consumerKey: "",
//            //    consumerSecret: "");

//            //app.UseFacebookAuthentication(
//            //    appId: "",
//            //    appSecret: "");

//            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
//            //{
//            //    ClientId = "",
//            //    ClientSecret = ""
//            //});
//        }
//    }
//}
