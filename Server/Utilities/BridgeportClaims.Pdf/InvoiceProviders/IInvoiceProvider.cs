using BridgeportClaims.Common.Models;

namespace BridgeportClaims.Pdf.InvoiceProviders
{
    public interface IInvoiceProvider
    {
        bool ProcessInvoice(InvoicePdfModel data, string targetPath);
    }
}