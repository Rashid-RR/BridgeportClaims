using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.Reports;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Excel.ExcelPackageFactory;
using BridgeportClaims.Web.CustomActionResults;
using BridgeportClaims.Web.Models;
using FluentNHibernate.Automapping;
using ServiceStack;

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
                return Ok(_reportsDataProvider.GetAccountsReceivableReport(model.GroupName, model.PharmacyName));
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
                var table = new DataTable();
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
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
