using System;
using System.ServiceProcess;
using System.Threading;
using BridgeportClaims.FileWatcherBusiness.Extensions;
using BridgeportClaims.FileWatcherBusiness.IO;
using BridgeportClaims.FileWatcherBusiness.Logging;
using BridgeportClaims.FileWatcherBusiness.Proxy;
using cs = BridgeportClaims.FileWatcherBusiness.ConfigService.ConfigService;
using c = BridgeportClaims.FileWatcherBusiness.StringConstants.Constants;

namespace BridgeportClaimsService.FileWatcherService
{
    internal static class Program
    {
        private const string False = "false";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            #if DEBUG
            var setting = cs.GetAppSetting(c.PerformInitialDirectoryTraversalKey);
            var initial = Convert.ToBoolean(setting.IsNotNullOrWhiteSpace() ? setting.ToLower() : False);
            var fileLocation = cs.GetAppSetting(c.FileLocationKey);
            if (initial)
            {
                var dt = IoHelper.TraverseDirectories(fileLocation)?.ToDataTable();
                if (null != dt)
                {
                    new ProxyProvider().MergeDocuments(dt);
                }
            }

            var service = new BridgeportClaimsWindowsService();
            service.OnDebug();
            Thread.Sleep(Timeout.Infinite);
            #else
            var loggingService = LoggingService.Instance;
            var logger = loggingService.Logger;
            try
            {
                var servicesToRun = new ServiceBase[]
                {
                    new BridgeportClaimsWindowsService()
                };
                ServiceBase.Run(servicesToRun);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
            #endif
        }
    }
}
