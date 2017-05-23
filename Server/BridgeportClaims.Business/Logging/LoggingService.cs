using System;
using Microsoft.ApplicationInsights;
using System.Collections.Generic;
using Microsoft.ApplicationInsights.DataContracts;

namespace BridgeportClaims.Business.Logging
{
    public class LoggingService : ILoggingService
    {
        #region Private Methods

        private static Dictionary<string, string> GetClassAndMethodNameProperties(string className = null,
            string methodName = null)
        {
            var classNamePopulated = !string.IsNullOrWhiteSpace(className);
            var methodNamePopulated = !string.IsNullOrWhiteSpace(methodName);
            if (!classNamePopulated && !methodNamePopulated)
                return null;
            var properties = new Dictionary<string, string>();
            if (classNamePopulated)
                properties.Add("ClassName", className);
            if (methodNamePopulated)
                properties.Add("MethodName", methodName);
            return properties;
        }

        private SeverityLevel GetSeverityLevelByLoggingLevel(LoggingLevel level)
        {
            switch (level)
            {
                case LoggingLevel.Verbose:
                    return SeverityLevel.Verbose;
                case LoggingLevel.Info:
                    return SeverityLevel.Information;
                case LoggingLevel.Warn:
                    return SeverityLevel.Warning;
                case LoggingLevel.Error:
                    return SeverityLevel.Error;
                case LoggingLevel.Fatal:
                    return SeverityLevel.Critical;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }

        private void Log(Exception ex = null, string className = null, string methodName = null, string message = null,
            LoggingLevel level = LoggingLevel.Verbose)
        {
            var telemetry = new TelemetryClient();
            var messagePopulated = !string.IsNullOrWhiteSpace(message);
            if (null == ex && !messagePopulated)
                throw new Exception("Error, cannot invoke the \"Log\" method with a null Exception and message.");
            if (null == ex)
                telemetry.TrackTrace(message, GetSeverityLevelByLoggingLevel(level),
                    GetClassAndMethodNameProperties(className, methodName));
            else
                telemetry.TrackException(ex, GetClassAndMethodNameProperties(className, methodName));
        }

        #endregion

        #region Public Methods

        public void Verbose(string message, string className = null, string methodName = null)
        {
            Log(null, className, methodName, message, LoggingLevel.Verbose);
        }

        public void Info(string message, string className = null, string methodName = null)
        {
            Log(null, className, methodName, message, LoggingLevel.Info);
        }

        public void Warn(string message, string className = null, string methodName = null)
        {
            Log(null, className, methodName, message, LoggingLevel.Warn);
        }

        public void Error(string message, string className = null, string methodName = null)
        {
            Log(null, className, methodName, message, LoggingLevel.Error);
        }

        public void Error(Exception ex, string className = null, string methodName = null)
        {
            Log(ex, className, methodName);
        }

        public void Fatal(string message, string className = null, string methodName = null)
        {
            Log(null, className, methodName, message, LoggingLevel.Fatal);
        }

        #endregion
    }
}
