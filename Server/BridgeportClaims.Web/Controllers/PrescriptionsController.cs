using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Business.BillingStatement;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.Claims;
using BridgeportClaims.Data.DataProviders.Prescriptions;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Data.Enums;
using BridgeportClaims.Pdf.Factories;
using BridgeportClaims.Web.CustomActionResults;
using BridgeportClaims.Web.Models;
using c = BridgeportClaims.Common.StringConstants.Constants;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/prescriptions")]
    public class PrescriptionsController : BaseApiController
    {
        private readonly IClaimsDataProvider _claimsDataProvider;
        private readonly IPrescriptionsDataProvider _prescriptionsDataProvider;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IPdfFactory _pdfFactory;
        private readonly IBillingStatementProvider _billingStatementProvider;

        public PrescriptionsController(
            IClaimsDataProvider claimsDataProvider,
            IPrescriptionsDataProvider prescriptionsDataProvider,
            IPdfFactory pdfFactory, 
            IBillingStatementProvider billingStatementProvider)
        {
            _claimsDataProvider = claimsDataProvider;
            _prescriptionsDataProvider = prescriptionsDataProvider;
            _pdfFactory = pdfFactory;
            _billingStatementProvider = billingStatementProvider;
        }

        [HttpPost]
        [Route("billing-statement")]
        public async Task<IHttpActionResult> GetBillingStatement(int claimId)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var fullFilePath = _billingStatementProvider.GenerateBillingStatementFullFilePath(claimId, out var fileName);
                    return new FileResult(fullFilePath, fileName, c.ExcelContentType);
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("multi-page-invoices")]
        public async Task<IHttpActionResult> GetMultiPageInvoices(MultiPageInvoicesModel model)
        {
            try
            {
                if (null == model)
                    throw new Exception("Error. No data was provided for this method.");
                return await Task.Run(() =>
                {
                    IList<PrescriptionIdDto> dto = new List<PrescriptionIdDto>();
                    model.PrescriptionIds.ForEach(x => dto.Add(GetPrescriptionIdDto(x)));
                    var fileUrls = _prescriptionsDataProvider.GetFileUrlsFromPrescriptionIds(dto);
                    var fileName = "Invoices_" + $"{DateTime.Now:yyyy-MM-dd_hh-mm-ss-tt}.pdf";
                    var targetPdf = Path.Combine(Path.GetTempPath(), fileName);
                    if (_pdfFactory.MergePdfs(fileUrls.ForEach(x => x.ToAbsoluteUri()), targetPdf))
                        return new DisplayFileResult(targetPdf, fileName, "application/pdf");
                    throw new Exception("The merge PDF's method failed.");
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        private static PrescriptionIdDto GetPrescriptionIdDto(int i) => new PrescriptionIdDto {PrescriptionID = i};

        [HttpPost]
        [Route("unpaid")]
        public IHttpActionResult GetUnpaidScripts(UnpaidScriptsViewModel model)
        {
            try
            {
                var list = _prescriptionsDataProvider.GetUnpaidScripts(model.IsDefaultSort, model.StartDate.ToNullableFormattedDateTime(), 
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
                    var operation = _prescriptionsDataProvider.AddOrUpdatePrescriptionStatus(prescriptionId, prescriptionStatusId);
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