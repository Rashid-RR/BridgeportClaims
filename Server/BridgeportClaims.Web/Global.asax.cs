﻿using System;
using System.Web.Http;
using BridgeportClaims.Services.Config;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using NLog;

namespace BridgeportClaims.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AutomapperStartup.Configure();
            NinjectWebCommon.RegisterNinject(GlobalConfiguration.Configuration);
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
