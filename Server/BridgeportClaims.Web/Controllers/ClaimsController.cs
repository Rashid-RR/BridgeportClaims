using System;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders;
using BridgeportClaims.Web.Models;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    public class ClaimsController : ApiController
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
                var claimsData = _getClaimsDataProvider.GetClaimsData(model.ClaimNumber,
                    model.FirstName, model.LastName, model.RxNumber, model.InvoiceNumber);
                return Ok(claimsData);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
