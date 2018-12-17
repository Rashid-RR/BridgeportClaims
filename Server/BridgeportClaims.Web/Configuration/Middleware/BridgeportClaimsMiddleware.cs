using NLog;
using System;
using Microsoft.Owin;
using System.Threading.Tasks;

namespace BridgeportClaims.Web.Middleware
{
    public class BridgeportClaimsMiddleware : OwinMiddleware
    {
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);
        public BridgeportClaimsMiddleware(OwinMiddleware next) : base(next) { }

        public override async Task Invoke(IOwinContext context)
        {
            try
            {
                await Next.Invoke(context);
            }
            catch (Exception ex)
            {
                Logger.Value.Fatal(ex);
                throw;
            }
        }
    }
}