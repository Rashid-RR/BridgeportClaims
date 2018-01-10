using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

                var month12 = new DateTime(now.Year, now.Month, 1);
                var month11 = month12.AddMonths(-1);
                var month10 = month11.AddMonths(-1);
                var month9 = month10.AddMonths(-1);
                var month8 = month9.AddMonths(-1);
                var month7 = month8.AddMonths(-1);
                var month6 = month7.AddMonths(-1);
                var month5 = month6.AddMonths(-1);
                var month4 = month5.AddMonths(-1);
                var month3 = month4.AddMonths(-1);
                var month2 = month3.AddMonths(-1);
                var month1 = month2.AddMonths(-1);
                var dates = new List<DateTime>
                {
                    month1,
                    month2,
                    month3,
                    month4,
                    month5,
                    month6,
                    month7,
                    month8,
                    month9,
                    month10,
                    month11,
                    month12
                };
                var names = dates.Select<DateTime, string>(x =>
                {
                    var propertyInfo = x.GetType().GetProperty("Name");
                    if (null != propertyInfo)
                        return propertyInfo.GetValue(x.ToString("MM-yy")).ToString();
                    return string.Empty;
                });


                var query = from r in results
                    select new {T = r.MonthBilled, r.YearBilled, r.TotalInvoiced, names[0] = r.Mnth1};
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
