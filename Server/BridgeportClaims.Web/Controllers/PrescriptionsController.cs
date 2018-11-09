﻿using NLog;
using System;
using System.IO;
using System.Net;
using System.Data;
using System.Linq;
using System.Web.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using BridgeportClaims.Business.BillingStatement;
using BridgeportClaims.Common.Constants;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.Claims;
using BridgeportClaims.Data.DataProviders.CollectionAssignments;
using BridgeportClaims.Data.DataProviders.Prescriptions;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Data.Enums;
using BridgeportClaims.Pdf.Factories;
using BridgeportClaims.Web.CustomActionResults;
using BridgeportClaims.Web.Models;
using Microsoft.AspNet.Identity;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/prescriptions")]
    public class PrescriptionsController : BaseApiController
    {
        private readonly Lazy<IClaimsDataProvider> _claimsDataProvider;
        private readonly Lazy<IPrescriptionsDataProvider> _prescriptionsDataProvider;
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);
        private readonly Lazy<IPdfFactory> _pdfFactory;
        private readonly Lazy<IBillingStatementProvider> _billingStatementProvider;
        private readonly Lazy<ICollectionAssignmentProvider> _collectionAssignmentProvider;

        public PrescriptionsController(
            Lazy<IClaimsDataProvider> claimsDataProvider,
            Lazy<IPrescriptionsDataProvider> prescriptionsDataProvider,
            Lazy<IPdfFactory> pdfFactory,
            Lazy<IBillingStatementProvider> billingStatementProvider,
            Lazy<ICollectionAssignmentProvider> collectionAssignmentProvider)
        {
            _claimsDataProvider = claimsDataProvider;
            _prescriptionsDataProvider = prescriptionsDataProvider;
            _pdfFactory = pdfFactory;
            _billingStatementProvider = billingStatementProvider;
            _collectionAssignmentProvider = collectionAssignmentProvider;
        }

        [HttpPost]
        [Route("get-active-users")]
        public IHttpActionResult GetActiveUsers()
        {
            try
            {
                var users = _prescriptionsDataProvider.Value.GetActiveUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("get-carriers")]
        public IHttpActionResult GetCarriers()
        {
            try
            {
                IEnumerable<PayorsDto> payors = _prescriptionsDataProvider.Value.GetCarriers();
                return Ok(payors);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("assign-users-to-payors")]
        public async Task<IHttpActionResult> AssignUsersToPayors(CollectionAssignmentModel model)
        {
            try
            {
                if (null == model)
                    throw new ArgumentNullException(nameof(model));
                if (null == model.UserId)
                    throw new ArgumentNullException(nameof(model.UserId));
                if (null == model.PayorIds)
                    throw new ArgumentNullException(nameof(model.PayorIds));
                var user = await AppUserManager.FindByIdAsync(model.UserId).ConfigureAwait(false);
                if (default(int) == model.PayorIds.Count)
                {
                    Logger.Value.Info($"We are removing all associations (if there are any), from {user.FullName}");
                }
                IList<CarrierDto> carrierDtos =
                    model.PayorIds.Select(item => new CarrierDto { PayorID = item }).ToList();
                var dt = carrierDtos.ToFixedDataTable();
                var modifiedByUserId = User.Identity.GetUserId();
                _collectionAssignmentProvider.Value.MergeCollectionAssignments(model.UserId, modifiedByUserId, dt);
                return Ok(new {message = $"{user.FullName} was associated to the selected carriers successfully."});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("billing-statement-excel")]
        public IHttpActionResult GetExcelBillingStatement(int claimId)
        {
            try
            {
                var fullFilePath =
                    _billingStatementProvider.Value.GenerateBillingStatementFullFilePath(claimId, out var fileName);
                return new FileResult(fullFilePath, fileName, StringConstants.ExcelContentType);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        [HttpPost]
        [Route("set-multiple-prescription-statuses")]
        public IHttpActionResult SetMultiplePrescriptionStatuses(MultiplePrescriptionStatusesModel model)
        {
            try
            {
                if (null == model)
                    throw new Exception("Error. No data was provided for this method.");
                var count = model.PrescriptionIds?.Count;
                if (null == count)
                    throw new Exception("Error. Zero prescription Id's were passed in.");
                IList<PrescriptionIdDto> dto = new List<PrescriptionIdDto>();
                model.PrescriptionIds.ForEach(x => dto.Add(GetPrescriptionIdDto(x)));
                var dt = dto.ToFixedDataTable();
                var userId = User.Identity.GetUserId();
                _prescriptionsDataProvider.Value.SetMultiplePrescriptionStatuses(dt, model.PrescriptionStatusId,
                    userId);
                const string multiple = "The prescription statuses were saved successfully.";
                const string single = "The prescription status was saved successfully.";
                var retVal = count.Value < 2 ? single : multiple;
                return Ok(new {message = retVal});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("multi-page-invoices")]
        public IHttpActionResult GetMultiPageInvoices(PrescriptionIdsModel model)
        {
            try
            {
                if (null == model)
                {
                    throw new Exception("Error. No data was provided for this method.");
                }
                IList<PrescriptionIdDto> dto = new List<PrescriptionIdDto>();
                model.PrescriptionIds.ForEach(x => dto.Add(GetPrescriptionIdDto(x)));
                var fileUrls = _prescriptionsDataProvider.Value.GetFileUrlsFromPrescriptionIds(dto);
                var fileName = "Invoices_" + $"{DateTime.Now:yyyy-MM-dd_hh-mm-ss-tt}.pdf";
                var targetPdf = Path.Combine(Path.GetTempPath(), fileName);
                if (_pdfFactory.Value.MergePdfs(fileUrls.ForEach(x => x.ToAbsoluteUri()), targetPdf))
                {
                    return new DisplayFileResult(targetPdf, fileName, "application/pdf");
                }
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
                if (null != model.PayorIds && model.PayorIds.Any())
                {
                    IList<CarrierDto> carrierDtos =
                        model.PayorIds.Select(item => new CarrierDto {PayorID = item}).ToList();
                    var dt = carrierDtos.ToFixedDataTable();
                    return ReturnUnpaidScripts(model.IsDefaultSort,
                        model.StartDate.ToNullableFormattedDateTime(),
                        model.EndDate.ToNullableFormattedDateTime(), model.Sort, model.SortDirection, model.Page,
                        model.PageSize, model.IsArchived, dt);
                }
                var dataTable = new DataTable();
                dataTable.Columns.Add("PayorID");
                return ReturnUnpaidScripts(model.IsDefaultSort,
                    model.StartDate.ToNullableFormattedDateTime(),
                    model.EndDate.ToNullableFormattedDateTime(), model.Sort, model.SortDirection, model.Page,
                    model.PageSize, model.IsArchived, dataTable);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        private IHttpActionResult ReturnUnpaidScripts(bool isDefaultSort, DateTime? startDate, DateTime? endDate,
            string sort, string sortDirection, int page, int pageSize, bool isArchived, DataTable carriers)
        {
            var results = _prescriptionsDataProvider.Value.GetUnpaidScripts(isDefaultSort, startDate, endDate, sort, sortDirection,
                page, pageSize, isArchived, carriers);
            return Ok(results);
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
                var userId = User.Identity.GetUserId();
                var msg = string.Empty;
                var operation = _prescriptionsDataProvider.Value.AddOrUpdatePrescriptionStatus(prescriptionId, prescriptionStatusId, userId);
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