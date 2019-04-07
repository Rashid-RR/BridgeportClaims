using System;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.InvoicePdfDocuments;
using BridgeportClaims.Pdf.InvoiceProviders;
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

        [HttpGet]
        [AllowAnonymous]
        [Route("process-invoice")]
        public IHttpActionResult ProcessInvoice()
        {
            try
            {
                var data = _invoicePdfDocumentProvider.Value.GetInvoicePdfDocument();
                _invoiceProvider.Value.ProcessInvoice(data);
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
