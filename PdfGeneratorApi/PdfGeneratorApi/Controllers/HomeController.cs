using NLog;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Http;
using PdfGeneratorApi.Data.DataProviders.InvoicePdfDocuments;
using PdfGeneratorApi.Pdf.InvoiceProviders;
using cs = PdfGeneratorApi.Common.Config.ConfigService;

namespace PdfGeneratorApi.Controllers
{
    [RoutePrefix("api/home")]
    public class HomeController : ApiController
    {
        private readonly Lazy<IInvoiceProvider> _invoiceProvider;
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);
        private readonly Lazy<IInvoicePdfDocumentProvider> _invoicePdfDocumentProvider;

        public HomeController(Lazy<IInvoiceProvider> invoiceProvider, Lazy<IInvoicePdfDocumentProvider> invoicePdfDocumentProvider)
        {
            _invoiceProvider = invoiceProvider;
            _invoicePdfDocumentProvider = invoicePdfDocumentProvider;
        }

        [HttpPost]
        [Route("generate-pdfs")]
        public IHttpActionResult GeneratePdfs(string userId)
        {
            try
            {
                var successCount = 0;
                while (true)
                {
                    var data = _invoicePdfDocumentProvider.Value.GetInvoicePdfDocument(userId)?.ToList();
                    if (null == data || !data.Any())
                    {
                        break;
                    }
                    var model = _invoicePdfDocumentProvider.Value.GetInvoicePdfModel(data.ToList());
                    var invoiceNumber = model.InvoiceNumber;
                    var fileName = $"{invoiceNumber}.pdf";
                    var targetPath = Path.Combine(cs.PdfDropPath, fileName);
                    if (_invoiceProvider.Value.ProcessInvoice(model, targetPath))
                    {
                        successCount++;
                    }
                    else
                    {
                        break;
                    }
                }
                var msg = successCount == 0
                    ? "No data to generate PDF's."
                    : successCount > 0
                        ? $"{successCount} PDF's were generated successfully."
                        : string.Empty;
                return Ok(new {message = msg});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
