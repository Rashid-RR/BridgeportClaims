using NLog;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.InvoicePdfDocuments;
using BridgeportClaims.Data.DataProviders.InvoicesProvider;
using BridgeportClaims.Pdf.InvoiceProviders;
using BridgeportClaims.Web.CustomActionResults;

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
        [Route("process-invoice")]
        public IHttpActionResult ProcessInvoice()
        {
            try
            {
                var data = _invoicePdfDocumentProvider.Value.GetInvoicePdfDocument();
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
    }
}
