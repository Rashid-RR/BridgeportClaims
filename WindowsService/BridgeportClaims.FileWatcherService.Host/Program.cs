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
                        Logger.Value.Info("Test");
                        svcCfg.ConstructUsing(() => container.Resolve<IFileWatcherWindowsService>());
                        svcCfg.WhenStarted(fileWatcherWindowsService => fileWatcherWindowsService.Start());
                        svcCfg.WhenStopped(fileWatcherWindowsService => fileWatcherWindowsService.Stop());
                        svcCfg.WhenShutdown(fileWatcherWindowsService => fileWatcherWindowsService.Stop());
                    });
                    hostCfg.OnException(ex => { Logger.Value.Error(ex); });
                    #if DEBUG
                    hostCfg.RunAsLocalSystem();
                    #else
                    hostCfg.StartAutomatically();
                    hostCfg.EnableServiceRecovery(rx => { rx.RestartService(1); });
                    hostCfg.RunAsNetworkService();
                    #endif
                    hostCfg.SetDescription("Bridgeport Claims File Watcher Service");
                    hostCfg.SetDisplayName("Bridgeport Claims File Watcher Service");
                    hostCfg.SetServiceName("Bridgeport File Watcher");
                });
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                throw;
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
