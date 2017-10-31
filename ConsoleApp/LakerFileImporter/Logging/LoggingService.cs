using System;
using NLog;

namespace LakerFileImporter.Logging
{
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
