﻿using System.ServiceProcess;
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
            var service = new BridgeportClaimsWindowsService();
            service.OnDebug();
            var servicesToRun = new ServiceBase[]
            {
                new BridgeportClaimsWindowsService()
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
