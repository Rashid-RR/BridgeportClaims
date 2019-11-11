using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.InvoicePdfDocuments;
using BridgeportClaims.Data.DataProviders.InvoicesProvider;
using cs = BridgeportClaims.Common.Config.ConfigService;
using s = BridgeportClaims.Common.Constants.StringConstants;
using BridgeportClaims.Pdf.InvoiceProviders;
using BridgeportClaims.Web.CustomActionResults;
using Microsoft.AspNet.Identity;
using ServiceStack;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "Indexer,Admin")]
    [RoutePrefix("api/invoices")]
    public class InvoicesController : BaseApiController
    {
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);
        private readonly Lazy<IInvoiceProvider> _invoiceProvider;
        private readonly Lazy<IInvoicesProvider> _invoicesProvider;
        private readonly Lazy<IInvoicePdfDocumentProvider> _invoicePdfDocumentProvider;

        public InvoicesController(Lazy<IInvoiceProvider> invoiceProvider,
            Lazy<IInvoicePdfDocumentProvider> invoicePdfDocumentProvider,
            Lazy<IInvoicesProvider> invoicesProvider)
        {
            _invoiceProvider = invoiceProvider;
            _invoicePdfDocumentProvider = invoicePdfDocumentProvider;
            _invoicesProvider = invoicesProvider;
        }

        [HttpPost]
        [Route("get-invoices")]
        public IHttpActionResult GetInvoices()
        {
            try
            {
                var data = _invoicesProvider.Value.GetInvoices();
                return Ok(data?.OrderByDescending(x => x.InvoiceDate));
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("get-invoice-processes")]
        public IHttpActionResult GetInvoiceProcesses()
        {
            try
            {
                var data = _invoicesProvider.Value.GetInvoiceProcesses();
                return Ok(data?.OrderByDescending(x => x.RxDate));
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("process-invoice-old")]
        public IHttpActionResult ProcessInvoiceOld(string userId)
        {
            try
            {
                var data = _invoicePdfDocumentProvider.Value.GetInvoicePdfDocument(userId);
                var model = _invoicePdfDocumentProvider.Value.GetInvoicePdfModel(data.ToList());
                var fileName = "Invoice_" + $"{DateTime.Now:yyyy-MM-dd_hh-mm-ss-tt}.pdf";
                var targetPath = Path.Combine(Path.GetTempPath(), fileName);
                if (_invoiceProvider.Value.ProcessInvoice(model, targetPath))
                {
                    return new FileResult(targetPath, fileName, "application/pdf");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("process-invoice")]
        public async Task<IHttpActionResult> ProcessInvoice()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var baseUrl = cs.GetAppSetting(s.PdfApiUrlKey);
                var path = cs.GetAppSetting(s.PdfApiUrlPath);
                var url = path + userId;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>(string.Empty, string.Empty)
                    });
                    var result = await client.PostAsync(url, content);
                    var msgJson = await result.Content.ReadAsStringAsync();
                    var message = msgJson.FromJson<ResponseModel>();
                    return Ok(new {message = message.Message});
                }
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }

    internal class ResponseModel
    {
        public string Message { get; set; }
    }
}
