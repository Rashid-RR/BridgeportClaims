using System;
using System.Diagnostics.CodeAnalysis;
using NLog;

namespace BridgeportClaims.FileWatcherBusiness.Logging
{
    [SuppressMessage("ReSharper", "ConvertToAutoProperty")]
    public sealed class LoggingService
    {
        private LoggingService()
        {
            LoggingConfigurator.ConfigureNLog();
            _logger = LogManager.GetCurrentClassLogger();
        }

        private static readonly Lazy<LoggingService> Lazy = new Lazy<LoggingService>(() => new LoggingService());

        public static LoggingService Instance => Lazy.Value;

        private readonly Logger _logger;

        public Logger Logger => _logger;
    }
}
