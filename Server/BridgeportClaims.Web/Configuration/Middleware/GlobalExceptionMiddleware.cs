using System;
using NLog;
using System.Threading.Tasks;
using Microsoft.Owin;


namespace BridgeportClaims.Web.Middleware
{
    public class GlobalExceptionMiddleware : OwinMiddleware
    {
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);
        public GlobalExceptionMiddleware(OwinMiddleware next) : base(next) { }
        public override async Task Invoke(IOwinContext context)
        {
            try
            {
                await Next.Invoke(context);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                throw;
            }
        }
    }
}