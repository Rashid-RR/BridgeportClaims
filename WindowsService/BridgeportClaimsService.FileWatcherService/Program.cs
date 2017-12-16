using System;
using System.ServiceProcess;
using System.Threading;
using BridgeportClaims.FileWatcherBusiness.Logging;

namespace BridgeportClaimsService.FileWatcherService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            #if DEBUG
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
