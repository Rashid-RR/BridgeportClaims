using System.Web.Http.ExceptionHandling;
using Microsoft.ApplicationInsights;

namespace BridgeportClaims.Web
{
    public class AiExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            if (null != context && null != context.Exception)
            {
                // Note: A single instance of telemetry client is sufficient to track multiple telemetry items.
                var ai = new TelemetryClient();
                ai.TrackException(context.Exception);
            }
            base.Log(context);
        }
    }
}