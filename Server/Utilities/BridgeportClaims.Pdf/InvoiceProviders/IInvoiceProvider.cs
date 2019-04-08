using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Pdf.InvoiceProviders
{
    public interface IInvoiceProvider
    {
        bool ProcessInvoice(InvoicePdfDto data, string targetPath);
    }
}