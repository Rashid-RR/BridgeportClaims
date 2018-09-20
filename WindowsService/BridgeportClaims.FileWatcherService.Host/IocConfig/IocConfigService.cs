using Autofac;
using BridgeportClaims.Business.Logging;
using BridgeportClaims.Business.Providers;
using BridgeportClaims.Business.Proxy;
using BridgeportClaims.FileWatcherService.Host.Services;
using NLog;

namespace BridgeportClaims.FileWatcherService.Host.IocConfig
{
    public class IocConfigService
    {
        public static IContainer Initialize()
        {
            // Dependency Injection \ IOC
            var builder = new ContainerBuilder();
            builder.RegisterType<FileWatcherWindowsService>().As<IFileWatcherWindowsService>().InstancePerDependency();
            builder.RegisterType<ProxyProvider>().As<IProxyProvider>().InstancePerDependency();
            builder.Register(ctx => LoggingService.Instance.Logger).As<ILogger>().SingleInstance();
            return builder.Build();
        }
    }
}