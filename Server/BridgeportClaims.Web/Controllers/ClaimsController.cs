using System;
using System.Linq;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders;
using BridgeportClaims.Web.Models;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    public class ClaimsController : BaseApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IGetClaimsDataProvider _getClaimsDataProvider;

        public ClaimsController(IGetClaimsDataProvider getClaimsDataProvider)
        {
            _getClaimsDataProvider = getClaimsDataProvider;
        }

        [HttpPost]
        public IHttpActionResult GetClaimsData([FromBody] ClaimsSearchViewModel model)
        {
            try
            {
                if (model.ClaimId == 0)
                    return InternalServerError(new Exception("Error, cannot pass in a Claim ID value of zero."));
                if (null != model.ClaimId) return GetClaimsDataByClaimId(model.ClaimId.Value);

                // Search terms passed, so we're at least performing a search first to see if multiple results appear.
                var claimsData = _getClaimsDataProvider.GetClaimsData(model.ClaimNumber,
                    model.FirstName, model.LastName, model.RxNumber, model.InvoiceNumber);
                if (null != claimsData && claimsData.Count == 1)
                    return GetClaimsDataByClaimId(claimsData.Single().ClaimId);
                return Ok(claimsData);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        private IHttpActionResult GetClaimsDataByClaimId(int claimId) => Ok(_getClaimsDataProvider.GetClaimsDataByClaimId(claimId));
    }
}
