using System;
using System.Web.Http;
using BridgeportClaims.Business.Config;
using HibernatingRhinos.Profiler.Appender.NHibernate;

namespace BridgeportClaims.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AutomapperStartup.Configure();
            var configService = new ConfigService();
            if (Convert.ToBoolean(configService.GetConfigItem("ApplicationIsInDebugMode")))
                NHibernateProfiler.Initialize();
        }
    }
}
