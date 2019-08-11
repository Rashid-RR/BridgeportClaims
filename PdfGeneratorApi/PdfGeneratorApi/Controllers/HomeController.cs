using System;
using System.Web.Http;
using PdfGeneratorApi.Pdf.InvoiceProviders;

namespace PdfGeneratorApi.Controllers
{
    [RoutePrefix("api/home")]
    public class HomeController : ApiController
    {
        private readonly Lazy<IInvoiceProvider> _invoiceProvider;

        public HomeController(Lazy<IInvoiceProvider> invoiceProvider)
        {
            _invoiceProvider = invoiceProvider;
        }

        [HttpGet]
        [Route("generate-pdfs")]
        public IHttpActionResult GeneratePdfs(string userId)
        {
            // var d = _invoiceProvider.Value.ProcessInvoice()
            return Ok(_invoiceProvider.Value.Boo());
        }
    }
}
