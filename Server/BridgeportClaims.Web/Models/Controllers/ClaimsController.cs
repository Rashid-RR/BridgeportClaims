using System;
using System.Reflection;
using System.Web.Http;
using BridgeportClaims.Business.Logging;
using BridgeportClaims.Data.DataProviders;
using BridgeportClaims.Web.Models;

namespace BridgeportClaims.Web.Controllers
{
    public class ClaimsController : ApiController
    {
        private readonly IGetClaimsDataProvider _getClaimsDataProvider;
        private readonly ILoggingService _loggingService;

        public ClaimsController(IGetClaimsDataProvider getClaimsDataProvider, ILoggingService loggingService)
        {
            _getClaimsDataProvider = getClaimsDataProvider;
            _loggingService = loggingService;
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
                _loggingService.Error(ex, this.GetType().Name, MethodBase.GetCurrentMethod()?.Name);
                throw;
            }
        }
    }
}
