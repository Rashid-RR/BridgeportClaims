using System;
using System.Net.Http.Headers;
using System.Web.Http.Filters;

namespace BridgeportClaims.Web.Framework.Attributes
{
    public class NoCacheAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext ctx)
        {
            ctx.Response.Headers.CacheControl = new CacheControlHeaderValue
            {
                Public = true,
                MaxAge = TimeSpan.FromSeconds(0),
                NoCache = true,
                MustRevalidate = true,
                NoStore = true
            };
            base.OnActionExecuted(ctx);
        }
    }
}