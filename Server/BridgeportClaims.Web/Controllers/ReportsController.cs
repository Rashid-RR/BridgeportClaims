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
using BridgeportClaims.Excel.ExcelPackageFactory;
using BridgeportClaims.Web.CustomActionResults;
using BridgeportClaims.Web.Models;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/reports")]
    public class ReportsController : BaseApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IReportsDataProvider _reportsDataProvider;
        private const string Format = "MMMM_yy";

        public ReportsController(IReportsDataProvider reportsDataProvider)
        {
            _reportsDataProvider = reportsDataProvider;
        }

        [HttpPost]
        [Route("accounts-receivable")]
        public IHttpActionResult GetAccountsReceivableReport(AccountsReceivableViewModel model)
        {
            try
            {
                var results = _reportsDataProvider.GetAccountsReceivableReport(model.GroupName, model.PharmacyName);
                var now = DateTime.Now.ToLocalTime();
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

                var selectStatement = $"new ( MonthBilled, YearBilled, TotalInvoiced, Mnth1 as {month1}, Mnth2 as {month2}" +
                    $", Mnth3 as {month3}, Mnth4 as {month4}, Mnth5 as {month5}, Mnth6 as {month6}, Mnth7 as {month7}, Mnth8 as {month8}" +
                    $", Mnth9 as {month9}, Mnth10 as {month10}, Mnth11 as {month11}, Mnth12 as {month12} )";
                var retVal = results?.AsQueryable().Select(selectStatement);
                return Ok(retVal);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("group-name")]
        public IHttpActionResult GetGroupNameAutoSuggest(string groupName)
        {
            try
            {
                return Ok(_reportsDataProvider.GetGroupNames(groupName));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("pharmacy-name")]
        public IHttpActionResult GetPharmacyNameAutoSuggest(string pharmacyName)
        {
            try
            {
                return Ok(_reportsDataProvider.GetPharmacyNames(pharmacyName));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("excel-export")]
        public IHttpActionResult ExportExcel(AccountsReceivableViewModel model)
        {
            try
            {
                return DisposableService.Using(() => new DataTable(), table =>
                {
                    table.Columns.Add("MonthBilled", typeof(string));
                    table.Columns.Add("TotalInvoiced", typeof(decimal));
                    table.Columns.Add("Jan17", typeof(decimal));
                    table.Columns.Add("Feb17", typeof(decimal));
                    table.Columns.Add("Mar17", typeof(decimal));
                    table.Columns.Add("Apr17", typeof(decimal));
                    table.Columns.Add("May17", typeof(decimal));
                    table.Columns.Add("Jun17", typeof(decimal));
                    table.Columns.Add("Jul17", typeof(decimal));
                    table.Columns.Add("Aug17", typeof(decimal));
                    table.Columns.Add("Sep17", typeof(decimal));
                    table.Columns.Add("Oct17", typeof(decimal));
                    table.Columns.Add("Nov17", typeof(decimal));
                    table.Columns.Add("Dec17", typeof(decimal));
                    var report = _reportsDataProvider.GetAccountsReceivableReport(model.GroupName, model.PharmacyName);
                    var dt = report.CopyToGenericDataTable(table, LoadOption.PreserveChanges);
                    if (null == dt)
                        throw new Exception("Could not create a data table from the report.");
                    var excelFactory = new ExcelFactory();
                    var fileName = "AccountsReceivableReport" + $"{DateTime.Now:yyyy-MM-dd_hh-mm-ss-tt}.xlsx";
                    var fullFilePath = excelFactory.GetExcelFilePathFromDataTable(dt, "Accounts Receivable", fileName);
                    return new FileResult(fullFilePath, fileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
