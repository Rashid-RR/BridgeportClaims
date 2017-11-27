using System.ServiceProcess;
using System.Threading;

namespace BridgeportClaimsService.FileWatcherService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        internal static void Main()
        {
#if DEBUG
            var service = new BridgeportClaimsWindowsService();
            service.OnDebug();
            Thread.Sleep(Timeout.Infinite);
#else
            var servicesToRun = new ServiceBase[]
            {
                new BridgeportClaimsWindowsService()
            };
            ServiceBase.Run(servicesToRun);
#endif
        }
    }
}
