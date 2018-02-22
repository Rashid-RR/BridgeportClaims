using NLog;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Business.PrescriptionReports;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.Claims;
using BridgeportClaims.Data.DataProviders.Prescriptions;
using BridgeportClaims.Data.Enums;
using BridgeportClaims.Web.CustomActionResults;
using BridgeportClaims.Web.Models;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/prescriptions")]
    public class PrescriptionsController : BaseApiController
    {
        private readonly IClaimsDataProvider _claimsDataProvider;
        private readonly IPrescriptionsProvider _prescriptionsProvider;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IPrescriptionReportFactory _prescriptionReportFactory;

        public PrescriptionsController(
            IClaimsDataProvider claimsDataProvider,
            IPrescriptionsProvider prescriptionsProvider, 
            IPrescriptionReportFactory prescriptionReportFactory)
        {
            _claimsDataProvider = claimsDataProvider;
            _prescriptionsProvider = prescriptionsProvider;
            _prescriptionReportFactory = prescriptionReportFactory;
        }

        [HttpPost]
        [Route("scripts-pdf")]
        public async Task<IHttpActionResult> GetPrescriptionsPdf(int claimId)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var path = _prescriptionReportFactory.GeneratePrescriptionReport(claimId);
                    var fileName = "Me.pdf";
                    return new FileResult(path, fileName, "application/pdf");
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("unpaid")]
        public IHttpActionResult GetUnpaidScripts(UnpaidScriptsViewModel model)
        {
            try
            {
                var list = _prescriptionsProvider.GetUnpaidScripts(model.IsDefaultSort, model.StartDate.ToNullableFormattedDateTime(), 
                    model.EndDate.ToNullableFormattedDateTime(), model.Sort, model.SortDirection, model.Page, model.PageSize);
                return Ok(list);
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