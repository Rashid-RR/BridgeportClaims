using System.ServiceProcess;
using System.Threading;

namespace BridgeportClaimsService
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
#endif
            var servicesToRun = new ServiceBase[]
            {
                new BridgeportClaimsWindowsService()
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
