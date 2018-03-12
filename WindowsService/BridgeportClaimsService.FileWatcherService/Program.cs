using System;
using System.ServiceProcess;
using System.Threading;
using BridgeportClaims.FileWatcherBusiness.Logging;
using NLog;

namespace BridgeportClaimsService.FileWatcherService
{
    internal static class Program
    {
        private static readonly LoggingService LoggingService = LoggingService.Instance;
        private static readonly Logger Logger = LoggingService.Logger;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            var currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += FileWatcherServiceUnhandledExceptionHandler;
            #if DEBUG
            var service = new BridgeportClaimsWindowsService();
            service.OnDebug();
            Thread.Sleep(Timeout.Infinite);
            #else
            
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
                Logger.Error(ex);
                throw;
            }
            #endif
        }

        private static void FileWatcherServiceUnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            var ex = (Exception) args.ExceptionObject;
            Logger.Fatal(ex, "A fatal exception was thrown.");
            throw ex;
        }
    }
}
