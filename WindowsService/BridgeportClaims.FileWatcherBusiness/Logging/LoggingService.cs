using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.NetworkInformation;
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

        public static string TimeFormat => "M/d/yyyy h:mm:ss tt";

        private readonly Logger _logger;

        public Logger Logger => _logger;
    }
}
