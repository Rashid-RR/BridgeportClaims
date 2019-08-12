using PdfGeneratorApi.Common.Models;

namespace PdfGeneratorApi.Pdf.InvoiceProviders
{
    public interface IInvoiceProvider
    {
        bool ProcessInvoice(InvoicePdfModel data, string targetPath);
    }
}