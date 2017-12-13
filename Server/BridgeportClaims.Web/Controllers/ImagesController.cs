using System;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.ClaimImages;
using BridgeportClaims.Web.Models;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/image")]
    public class ImagesController : BaseApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IClaimImageProvider _claimImageProvider;

        public ImagesController(IClaimImageProvider claimImageProvider)
        {
            _claimImageProvider = claimImageProvider;
        }

        [HttpPost]
        [Route("get")]
        public IHttpActionResult GetClaimImages(ClaimImageViewModel model)
        {
            try
            {
                var results = _claimImageProvider.GetClaimImages(model.Sort, model.SortDirection, model.Page, model.PageSize);
                return Ok(results);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
