using System;
using BridgeportClaims.Services.Config;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using NLog;

namespace BridgeportClaims.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //GlobalConfiguration.Configure(WebApiConfig.Register);
            AutomapperStartup.Configure();
            var configService = new ConfigService();
            if (Convert.ToBoolean(configService.ApplicationIsInDebugMode))
                NHibernateProfiler.Initialize();
        }

        protected void Application_Error()
        {
            var lastException = Server.GetLastError();
            var logger = LogManager.GetCurrentClassLogger();
            logger.Fatal(lastException);
        }
    }
}
