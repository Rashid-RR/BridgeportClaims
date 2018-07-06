using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.Reports;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Excel.Factories;
using BridgeportClaims.Web.CustomActionResults;
using BridgeportClaims.Web.Models;
using Microsoft.AspNet.Identity;
using c = BridgeportClaims.Common.StringConstants.Constants;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/reports")]
    public class ReportsController : BaseApiController
    {
        private static readonly Lazy<Logger> Logger = new Lazy<Logger>(LogManager.GetCurrentClassLogger);
        private readonly Lazy<IReportsDataProvider> _reportsDataProvider;
        private const string Format = "MMMM_yyyy";

        public ReportsController(Lazy<IReportsDataProvider> reportsDataProvider)
        {
            _reportsDataProvider = reportsDataProvider;
        }

        private static Dictionary<int, string> MonthDictionary
        {
            get
            {
                var now = DateTime.UtcNow.ToMountainTime();
                var thisMonth = new DateTime(now.Year, now.Month, 1);
                var month12 = thisMonth.ToString(Format);
                var month11 = thisMonth.AddMonths(-1).ToString(Format);
                var month10 = thisMonth.AddMonths(-2).ToString(Format);
                var month9 = thisMonth.AddMonths(-3).ToString(Format);
                var month8 = thisMonth.AddMonths(-4).ToString(Format);
                var month7 = thisMonth.AddMonths(-5).ToString(Format);
                var month6 = thisMonth.AddMonths(-6).ToString(Format);
                var month5 = thisMonth.AddMonths(-7).ToString(Format);
                var month4 = thisMonth.AddMonths(-8).ToString(Format);
                var month3 = thisMonth.AddMonths(-9).ToString(Format);
                var month2 = thisMonth.AddMonths(-10).ToString(Format);
                var month1 = thisMonth.AddMonths(-11).ToString(Format);
                var retVal = new Dictionary<int, string>
                {
                    {1, month1},
                    {2, month2},
                    {3, month3},
                    {4, month4},
                    {5, month5},
                    {6, month6},
                    {7, month7},
                    {8, month8},
                    {9, month9},
                    {10, month10},
                    {11, month11},
                    {12, month12}
                };
                return retVal;
            }
        }

        [HttpPost]
        [Route("skippedpayment")]
        public IHttpActionResult GetSkippedPayment(CarriersModel model)
        {
            try
            {
                if (null == model)
                    throw new ArgumentNullException(nameof(model));
                if (default (int) == model.Page || default (int) == model.PageSize)
                    throw new Exception($"Error, the {nameof(model.Page)} and {nameof(model.PageSize)} parameters had invalid values.");
                if (null != model.PayorIds && model.PayorIds.Any())
                {
                    IList<CarrierDto> carrierDtos = model.PayorIds.Select(item => new CarrierDto {PayorID = item}).ToList();
                    var dt = carrierDtos.ToFixedDataTable();
                    var results = _reportsDataProvider.Value.GetSkippedPaymentReport(model.Page, model.PageSize, dt);
                    return Ok(results);
                }
                var dataTable = new DataTable();
                dataTable.Columns.Add("PayorID");
                return Ok(_reportsDataProvider.Value.GetSkippedPaymentReport(model.Page, model.PageSize, dataTable));
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("shortpay")]
        public IHttpActionResult GetShortPayReport(PaginationModel model)
        {
            try
            {
                var results = _reportsDataProvider.Value.GetShortPayReport(model?.Sort, model?.SortDirection,
                    model?.Page ?? -1, model?.PageSize ?? -1);
                return Ok(results);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("remove-shortpay")]
        public IHttpActionResult RemoveShortPayReport(int prescriptionPaymentId)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var result = _reportsDataProvider.Value.RemoveShortPay(prescriptionPaymentId, userId);
                if (result)
                    return Ok(new { message = "This entry was removed from the short pay report successfully."});
                throw new Exception("This entry was not removed from the short pay report");
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("duplicate-claims")]
        public IHttpActionResult GetDuplicateClaims(PaginationModel model)
        {
            try
            {
                var retVal = _reportsDataProvider.Value.GetDuplicateClaims(model?.Sort, model?.SortDirection,
                    model?.Page ?? -1, model?.PageSize ?? -1);
                return Ok(retVal);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("accounts-receivable")]
        public IHttpActionResult GetAccountsReceivableReport(AccountsReceivableViewModel model)
        {
            try
            {
                var retVal = GetAccountsReceivableReport(model.GroupName, model.PharmacyName);
                return Ok(retVal);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        private IQueryable GetAccountsReceivableReport(string groupName, string pharmacyName)
        {
            try
            {
                var results = _reportsDataProvider.Value.GetAccountsReceivableReport(groupName, pharmacyName);
                var selectStatement = $"new ( DateBilled, TotalInvoiced, Mnth1 as {MonthDictionary[1]}, Mnth2 as {MonthDictionary[2]}" +
                                      $", Mnth3 as {MonthDictionary[3]}, Mnth4 as {MonthDictionary[4]}, Mnth5 as {MonthDictionary[5]}, " +
                                      $"Mnth6 as {MonthDictionary[6]}, Mnth7 as {MonthDictionary[7]}, Mnth8 as {MonthDictionary[8]}" +
                                      $", Mnth9 as {MonthDictionary[9]}, Mnth10 as {MonthDictionary[10]}, Mnth11 as {MonthDictionary[11]}, Mnth12 as {MonthDictionary[12]} )";
                var retVal = results?.AsQueryable().Select(selectStatement);
                return retVal;
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("group-name")]
        public IHttpActionResult GetGroupNameAutoSuggest(string groupName)
        {
            try
            {
                return Ok(_reportsDataProvider.Value.GetGroupNames(groupName));
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("pharmacy-name")]
        public IHttpActionResult GetPharmacyNameAutoSuggest(string pharmacyName)
        {
            try
            {
                return Ok(_reportsDataProvider.Value.GetPharmacyNames(pharmacyName));
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("excel-export")]
        public IHttpActionResult ExportExcel(AccountsReceivableViewModel model)
        {
            try
            {
                return DisposableService.Using(() => new DataTable(), table =>
                {
                    var report = GetAccountsReceivableReport(model.GroupName, model.PharmacyName);
                    if (null == report)
                        throw new Exception("Error. No results were found from running the Accounts Receivable report.");
                    var dataTable = report.ToDynamicLinqDataTable();
                    if (null == dataTable)
                        throw new Exception("Could not create a data table from the report.");
                    var fileName = "AccountsReceivableReport" + $"{DateTime.Now:yyyy-MM-dd_hh-mm-ss-tt}.xlsx";
                    var fullFilePath = ExcelFactory.GetExcelFilePathFromDataTable(dataTable, "Accounts Receivable", fileName);
                    return new FileResult(fullFilePath, fileName, c.ExcelContentType);
                });
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
