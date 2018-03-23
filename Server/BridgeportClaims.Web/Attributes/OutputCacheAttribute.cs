using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace BridgeportClaims.Web.Attributes
{
    public class BridgeportClaimsOutputCacheAttribute : ActionFilterAttribute
    {
        #region Private Members
        // cache length in seconds
        private readonly int _timespan;
        // client cache length in seconds
        private readonly int _clientTimeSpan;
        // cache for anonymous users only?
        private readonly bool _anonymousOnly;
        // cache key
        private string _cachekey;
        public string VaryByParam;

        // cache repository
        private static readonly ObjectCache WebApiCache = MemoryCache.Default;

        #endregion

        #region Ctor
        
        public BridgeportClaimsOutputCacheAttribute(int timespan, int clientTimeSpan, bool anonymousOnly)
        {
            _timespan = timespan;
            _clientTimeSpan = clientTimeSpan;
            _anonymousOnly = anonymousOnly;
        }

        #endregion

        #region Private Methods
        
        private bool _isCacheable(HttpActionContext ac)
        {
            if (_timespan > 0 && _clientTimeSpan > 0)
            {
                if (_anonymousOnly)
                    if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
                        return false;
                if (ac.Request.Method == HttpMethod.Get) return true;
            }
            else
            {
                throw new InvalidOperationException("Wrong Arguments");
            }
            return false;
        }

        private CacheControlHeaderValue SetClientCache()
        {
            var cachecontrol = new CacheControlHeaderValue
            {
                MaxAge = TimeSpan.FromSeconds(_clientTimeSpan),
                MustRevalidate = true
            };
            return cachecontrol;
        }

        #endregion

        #region Public Methods

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (null != actionContext)
            {
                if (!_isCacheable(actionContext)) return;
                var mediaTypeWithQualityHeaderValue = actionContext.Request.Headers.Accept.FirstOrDefault();
                if (mediaTypeWithQualityHeaderValue != null)
                    _cachekey = string.Join(":", actionContext.Request.RequestUri.AbsolutePath,
                        mediaTypeWithQualityHeaderValue.ToString());
                if (!WebApiCache.Contains(_cachekey)) return;
                var val = (string)WebApiCache.Get(_cachekey);
                if (val == null) return;
                actionContext.Response = actionContext.Request.CreateResponse();
                actionContext.Response.Content = new StringContent(val);
                var contenttype = (MediaTypeHeaderValue) WebApiCache.Get(_cachekey + ":response-ct") ??
                                  new MediaTypeHeaderValue(_cachekey.Split(':')[1]);
                actionContext.Response.Content.Headers.ContentType = contenttype;
                actionContext.Response.Headers.CacheControl = SetClientCache();
            }
            else
            {
                throw new ArgumentNullException(nameof(actionContext));
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (!(WebApiCache.Contains(_cachekey)))
            {
                var body = actionExecutedContext.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                WebApiCache.Add(_cachekey, body, DateTime.Now.AddSeconds(_timespan));
                WebApiCache.Add(_cachekey + ":response-ct", actionExecutedContext.Response.Content.Headers.ContentType,
                    DateTime.Now.AddSeconds(_timespan));
            }
            if (_isCacheable(actionExecutedContext.ActionContext))
                actionExecutedContext.ActionContext.Response.Headers.CacheControl = SetClientCache();
        }

        #endregion
    }
}