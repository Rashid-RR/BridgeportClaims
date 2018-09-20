using System;
using BridgeportClaims.Business.Logging;
using NLog;

namespace BridgeportClaims.FileWatcherService.Host
{
    public class Program
    {
        private static readonly LoggingService LoggingService = LoggingService.Instance;
        private static readonly Logger Logger = LoggingService.Logger;

        public static void Main(string[] args)
        {
            var currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += FileWatcherServiceUnhandledExceptionHandler;

        }

        private static void FileWatcherServiceUnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            var ex = (Exception)args.ExceptionObject;
            Logger.Fatal(ex, "A fatal exception was thrown.");
            throw ex;
        }
    }
}
