using System;
using System.ServiceProcess;
using System.Threading;
using BridgeportClaims.Business.Logging;
using NLog;

namespace BridgeportClaims.FileWatcherService
{
    static class Program
    {
        private static readonly LoggingService LoggingService = LoggingService.Instance;
        private static readonly Logger Logger = LoggingService.Logger;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += FileWatcherServiceUnhandledExceptionHandler;
            #if DEBUG
            var service = new ServiceInstaller();
            service.OnDebug();
            Thread.Sleep(Timeout.Infinite);
            #else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ServiceInstaller()
            };
            ServiceBase.Run(ServicesToRun);
            #endif
        }

        private static void FileWatcherServiceUnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            var ex = (Exception)args.ExceptionObject;
            Logger.Fatal(ex, "A fatal exception was thrown.");
            throw ex;
        }
    }
}
