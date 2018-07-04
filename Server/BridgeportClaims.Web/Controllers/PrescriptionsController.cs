using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
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
using Microsoft.AspNet.Identity;
using c = BridgeportClaims.Common.StringConstants.Constants;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/prescriptions")]
    public class PrescriptionsController : BaseApiController
    {
        private readonly Lazy<IClaimsDataProvider> _claimsDataProvider;
        private readonly Lazy<IPrescriptionsDataProvider> _prescriptionsDataProvider;
        private static readonly Lazy<Logger> Logger = new Lazy<Logger>(LogManager.GetCurrentClassLogger);
        private readonly Lazy<IPdfFactory> _pdfFactory;
        private readonly Lazy<IBillingStatementProvider> _billingStatementProvider;

        public PrescriptionsController(
            Lazy<IClaimsDataProvider> claimsDataProvider,
            Lazy<IPrescriptionsDataProvider> prescriptionsDataProvider,
            Lazy<IPdfFactory> pdfFactory,
            Lazy<IBillingStatementProvider> billingStatementProvider)
        {
            _claimsDataProvider = claimsDataProvider;
            _prescriptionsDataProvider = prescriptionsDataProvider;
            _pdfFactory = pdfFactory;
            _billingStatementProvider = billingStatementProvider;
        }

        [HttpPost]
        [Route("billing-statement-excel")]
        public IHttpActionResult GetExcelBillingStatement(int claimId)
        {
            try
            {
                var fullFilePath =
                    _billingStatementProvider.Value.GenerateBillingStatementFullFilePath(claimId, out var fileName);
                return new FileResult(fullFilePath, fileName, c.ExcelContentType);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        [HttpPost]
        [Route("multi-page-invoices")]
        public IHttpActionResult GetMultiPageInvoices(MultiPageInvoicesModel model)
        {
            try
            {
                if (null == model)
                    throw new Exception("Error. No data was provided for this method.");
                IList<PrescriptionIdDto> dto = new List<PrescriptionIdDto>();
                model.PrescriptionIds.ForEach(x => dto.Add(GetPrescriptionIdDto(x)));
                var fileUrls = _prescriptionsDataProvider.Value.GetFileUrlsFromPrescriptionIds(dto);
                var fileName = "Invoices_" + $"{DateTime.Now:yyyy-MM-dd_hh-mm-ss-tt}.pdf";
                var targetPdf = Path.Combine(Path.GetTempPath(), fileName);
                if (_pdfFactory.Value.MergePdfs(fileUrls.ForEach(x => x.ToAbsoluteUri()), targetPdf))
                    return new DisplayFileResult(targetPdf, fileName, "application/pdf");
                throw new Exception("The merge PDF's method failed.");
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        private static PrescriptionIdDto GetPrescriptionIdDto(int i) => new PrescriptionIdDto {PrescriptionID = i};

        [HttpPost]
        [Route("unpaid")]
        public IHttpActionResult GetUnpaidScripts(UnpaidScriptsViewModel model)
        {
            try
            {
                var list = _prescriptionsDataProvider.Value.GetUnpaidScripts(model.IsDefaultSort, model.StartDate.ToNullableFormattedDateTime(), 
                    model.EndDate.ToNullableFormattedDateTime(), model.Sort, model.SortDirection, model.Page, model.PageSize, model.IsArchived);
                return Ok(list);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("sort")]
        public IHttpActionResult SortPrescriptions(int claimId, string sort, string sortDirection, int page,
            int pageSize)
        {
            try
            {
                return Ok(_claimsDataProvider.Value.GetPrescriptionDataByClaim(claimId, sort, sortDirection, page, pageSize));
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        [HttpPost]
        [Route("set-status")]
        public IHttpActionResult SetPrescriptionStatus(int prescriptionId, int prescriptionStatusId)
        {
            try
            {
                var msg = string.Empty;
                var operation =
                    _prescriptionsDataProvider.Value.AddOrUpdatePrescriptionStatus(prescriptionId,
                        prescriptionStatusId);
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
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        [HttpPost]
        [Route("archive-unpaid-script")]
        public IHttpActionResult ArchiveUnpaidScript(ArchiveUnpaidScriptModel model)
        {
            try
            {
                if (null == model)
                    throw new ArgumentNullException(nameof(model));
                if (model.PrescriptionId == default (int))
                    throw new Exception("Error, the prescription Id supplied is not valid.");
                var userId = User.Identity.GetUserId();
                if (userId.IsNullOrWhiteSpace())
                    throw new ArgumentNullException(nameof(userId));
                _prescriptionsDataProvider.Value.ArchiveUnpaidScript(model.PrescriptionId, userId);
                return Ok(new {message = "Successfully archived this unpaid script."});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}