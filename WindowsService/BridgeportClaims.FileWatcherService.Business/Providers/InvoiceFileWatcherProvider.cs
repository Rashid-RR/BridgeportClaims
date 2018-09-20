using BridgeportClaims.Business.Enums;

namespace BridgeportClaims.Business.Providers
{
    public class InvoiceFileWatcherProvider : FileWatcherProvider
    {
        public InvoiceFileWatcherProvider() : base(FileType.Invoices) { }
    }
}