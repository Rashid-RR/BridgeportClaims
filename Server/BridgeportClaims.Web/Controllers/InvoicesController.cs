using System;
using System.IO;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.InvoicePdfDocuments;
using BridgeportClaims.Pdf.InvoiceProviders;
using BridgeportClaims.Web.CustomActionResults;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "Indexer,Admin")]
    [RoutePrefix("api/invoices")]
    public class InvoicesController : BaseApiController
    {
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);
        private readonly Lazy<IInvoiceProvider> _invoiceProvider;
        private readonly Lazy<IInvoicePdfDocumentProvider> _invoicePdfDocumentProvider;

        public InvoicesController(Lazy<IInvoiceProvider> invoiceProvider,
            Lazy<IInvoicePdfDocumentProvider> invoicePdfDocumentProvider)
        {
            _invoiceProvider = invoiceProvider;
            _invoicePdfDocumentProvider = invoicePdfDocumentProvider;
        }

        [HttpPost]
        [Route("process-invoice")]
        public IHttpActionResult ProcessInvoice()
        {
            try
            {
                var data = _invoicePdfDocumentProvider.Value.GetInvoicePdfDocument();
                var fileName = "Invoice_" + $"{DateTime.Now:yyyy-MM-dd_hh-mm-ss-tt}.pdf";
                var targetPath = Path.Combine(Path.GetTempPath(), fileName);
                if (_invoiceProvider.Value.ProcessInvoice(data, targetPath))
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
    }
}
