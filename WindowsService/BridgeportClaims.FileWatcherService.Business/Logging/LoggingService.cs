using System;
using System.Diagnostics.CodeAnalysis;
using BridgeportClaims.Business.Extensions;
using NLog;

namespace BridgeportClaims.Business.Logging
{
    [SuppressMessage("ReSharper", "ConvertToAutoProperty")]
    public sealed class LoggingService
    {
        private LoggingService()
        {
            LoggingConfigurator.ConfigureNLog();
            LogManager.ThrowExceptions = true;
            Logger = LogManager.GetCurrentClassLogger();
        }
        private static readonly Lazy<LoggingService> Lazy = new Lazy<LoggingService>(() => new LoggingService());
        public static LoggingService Instance => Lazy.Value;
        public static string TimeFormat => "M/d/yyyy h:mm:ss tt";
        public Logger Logger { get; }
        public void LogDebugMessage(string method, string now, string msg = null)
            => Logger.Info($"Debugging inside of the {method} method on {now}.{(msg.IsNotNullOrWhiteSpace() ? $" {msg}." : string.Empty)}");
    }
}
