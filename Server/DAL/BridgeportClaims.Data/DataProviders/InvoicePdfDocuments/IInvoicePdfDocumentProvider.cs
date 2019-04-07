using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.InvoicePdfDocuments
{
    public interface IInvoicePdfDocumentProvider
    {
        InvoicePdfDto GetInvoicePdfDocument();
    }
}