using System;
using Autofac;
using BridgeportClaims.Business.Logging;
using BridgeportClaims.FileWatcherService.Host.IocConfig;
using BridgeportClaims.FileWatcherService.Host.Services;
using NLog;
using Topshelf;

namespace BridgeportClaims.FileWatcherService.Host
{
    public class Program
    {
        private static readonly LoggingService LoggingService = LoggingService.Instance;
        private static readonly Lazy<Logger> Logger = new Lazy<Logger>(() => LoggingService.Logger);

        public static void Main(string[] args)
        {
            var currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += FileWatcherServiceUnhandledExceptionHandler;
            try
            {
                // IoC
                var container = IocConfigService.Initialize();

                HostFactory.Run(hostCfg =>
                {
                    hostCfg.Service<IFileWatcherWindowsService>(svcCfg =>
                    {
                        svcCfg.ConstructUsing(() => container.Resolve<IFileWatcherWindowsService>());
                        svcCfg.WhenStarted(fileWatcherWindowsService => fileWatcherWindowsService.Start());
                        svcCfg.WhenStopped(fileWatcherWindowsService => fileWatcherWindowsService.Stop());
                    });
                    hostCfg.OnException(ex => { Logger.Value.Error(ex); });
                    hostCfg.RunAsLocalSystem();
                    hostCfg.StartAutomatically();
                    hostCfg.EnableServiceRecovery(rx => { rx.RestartService(5); });
                    hostCfg.SetDescription("Bridgeport Claims File Watcher Service");
                    hostCfg.SetDisplayName("Bridgeport Claims File Watcher Service");
                    hostCfg.SetServiceName("Bridgeport File Watcher");
                });
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                Console.WriteLine(ex.Message);
            }
        }

        private static void FileWatcherServiceUnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            var ex = (Exception)args.ExceptionObject;
            Logger.Value.Fatal(ex, "A fatal exception was thrown.");
            throw ex;
        }
    }
}
