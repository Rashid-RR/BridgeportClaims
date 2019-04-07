using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Pdf.InvoiceProviders
{
    public interface IInvoiceProvider
    {
        void ProcessInvoice(InvoicePdfDto data);
    }
}