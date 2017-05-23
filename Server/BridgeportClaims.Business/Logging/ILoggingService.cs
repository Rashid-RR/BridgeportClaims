using System;

namespace BridgeportClaims.Business.Logging
{
    public interface ILoggingService
    {
        void Verbose(string message, string className = null, string methodName = null);
        void Info(string message, string className = null, string methodName = null);
        void Warn(string message, string className = null, string methodName = null);
        void Error(string message, string className = null, string methodName = null);
        void Error(Exception ex, string className = null, string methodName = null);
        void Fatal(string message, string className = null, string methodName = null);
    }
}