using System;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.AttorneyProviders;
using BridgeportClaims.Web.Models;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/attorney")]
    public class AttorneyController : BaseApiController
    {
        private readonly Lazy<IAttorneyProvider> _attorneyProvider;
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);

        public AttorneyController(Lazy<IAttorneyProvider> attorneyProvider)
        {
            _attorneyProvider = attorneyProvider;
        }

        [HttpPost]
        [Route("get-attorneys")]
        public IHttpActionResult GetAdjustorNames(AbstractSearchModel model)
        {
            try
            {
                var results = _attorneyProvider.Value.GetAttorneys(model.SearchText, model.Page, model.PageSize,
                    model.Sort
                    , model.SortDirection);
                return Ok(results);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
