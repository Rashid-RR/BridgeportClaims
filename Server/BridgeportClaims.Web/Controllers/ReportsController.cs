using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.Reports;
using BridgeportClaims.Excel.ExcelPackageFactory;
using BridgeportClaims.Pdf.ITextPdfFactory;
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
        private const string Format = "MMMM_yyyy";

        public ReportsController(IReportsDataProvider reportsDataProvider)
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
        [Route("scripts-pdf")]
        public async Task<IHttpActionResult> GetPrescriptionsPdf()
        {
            try
            {
                return await Task.Run(() =>
                {
                    var pdfFactory = new PdfFactory();
                    pdfFactory.GeneratePdf(new DataTable("tablename"));
                    const string msg = "The PDF was generated successfully";
                    return Ok(new {message = msg});
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("accounts-receivable")]
        public async Task<IHttpActionResult> GetAccountsReceivableReport(AccountsReceivableViewModel model)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var retVal = GetAccountsReceivableReport(model.GroupName, model.PharmacyName);
                    return Ok(retVal);
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        private IQueryable GetAccountsReceivableReport(string groupName, string pharmacyName)
        {
            try
            {
                var results = _reportsDataProvider.GetAccountsReceivableReport(groupName, pharmacyName);
                var selectStatement = $"new ( DateBilled, TotalInvoiced, Mnth1 as {MonthDictionary[1]}, Mnth2 as {MonthDictionary[2]}" +
                                      $", Mnth3 as {MonthDictionary[3]}, Mnth4 as {MonthDictionary[4]}, Mnth5 as {MonthDictionary[5]}, " +
                                      $"Mnth6 as {MonthDictionary[6]}, Mnth7 as {MonthDictionary[7]}, Mnth8 as {MonthDictionary[8]}" +
                                      $", Mnth9 as {MonthDictionary[9]}, Mnth10 as {MonthDictionary[10]}, Mnth11 as {MonthDictionary[11]}, Mnth12 as {MonthDictionary[12]} )";
                var retVal = results?.AsQueryable().Select(selectStatement);
                return retVal;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
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
                    var report = GetAccountsReceivableReport(model.GroupName, model.PharmacyName);
                    if (null == report)
                        throw new Exception("Error. No results were found from running the Accounts Receivable report.");
                    var dataTable = report.ToDynamicLinqDataTable();
                    if (null == dataTable)
                        throw new Exception("Could not create a data table from the report.");
                    var excelFactory = new ExcelFactory();
                    var fileName = "AccountsReceivableReport" + $"{DateTime.Now:yyyy-MM-dd_hh-mm-ss-tt}.xlsx";
                    var fullFilePath = excelFactory.GetExcelFilePathFromDataTable(dataTable, "Accounts Receivable", fileName);
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
