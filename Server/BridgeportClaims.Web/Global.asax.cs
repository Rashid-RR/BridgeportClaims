using System;
using System.Web.Http;
using System.Web.Mvc;
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
            var configService = DependencyResolver.Current.GetService(typeof(IConfigService)) as IConfigService;
            if (configService != null && Convert.ToBoolean(configService.GetConfigItem("ApplicationIsInDebugMode")))
                NHibernateProfiler.Initialize();
        }
    }
}
