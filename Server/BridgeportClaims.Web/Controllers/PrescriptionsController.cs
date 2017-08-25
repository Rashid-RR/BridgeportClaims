using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.Claims;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/prescriptions")]
    public class PrescriptionsController : BaseApiController
    {
        private readonly IClaimsDataProvider _claimsDataProvider;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public PrescriptionsController(IClaimsDataProvider claimsDataProvider)
        {
            _claimsDataProvider = claimsDataProvider;
        }

        [HttpPost]
        [Route("sort")]
        public async Task<IHttpActionResult> SortPrescriptions(int claimId, string sort, string sortDirection, int page,
            int pageSize)
        {
            try
            {
                return await Task.Run(() => Ok(
                    _claimsDataProvider.GetPrescriptionDataByClaim(claimId, sort, sortDirection, page, pageSize)));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }
    }
}
