using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.Claims;
using BridgeportClaims.Data.DataProviders.Prescriptions;
using BridgeportClaims.Data.Enums;
using BridgeportClaims.Web.Models;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/prescriptions")]
    public class PrescriptionsController : BaseApiController
    {
        private readonly IClaimsDataProvider _claimsDataProvider;
        private readonly IPrescriptionsProvider _prescriptionsProvider;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public PrescriptionsController(IClaimsDataProvider claimsDataProvider,
            IPrescriptionsProvider prescriptionsProvider)
        {
            _claimsDataProvider = claimsDataProvider;
            _prescriptionsProvider = prescriptionsProvider;
        }

        [HttpPost]
        [Route("unpaid")]
        public IHttpActionResult GetUnpaidScripts(UnpaidScriptsViewModel model)
        {
            try
            {
                return Ok(_prescriptionsProvider.GetUnpaidScripts(model.IsDefaultSort, model.StartDate, model.EndDate,
                    model.Sort, model.SortDirection, model.Page, model.PageSize));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
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
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        [HttpPost]
        [Route("set-status")]
        public async Task<IHttpActionResult> SetPrescriptionStatus(int prescriptionId, int prescriptionStatusId)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var msg = string.Empty;
                    var operation = _prescriptionsProvider.AddOrUpdatePrescriptionStatus(prescriptionId, prescriptionStatusId);
                    switch (operation)
                    {
                        case EntityOperation.Add:
                            msg = "The prescription's status was added successfully.";
                            break;
                        case EntityOperation.Update:
                            msg = "The prescription's status was updated successfully.";
                            break;
                        case EntityOperation.Delete:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    return Ok(new {message = msg});
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }
    }
}