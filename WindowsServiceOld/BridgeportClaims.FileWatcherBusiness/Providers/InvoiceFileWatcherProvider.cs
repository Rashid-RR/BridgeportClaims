using BridgeportClaims.FileWatcherBusiness.Enums;

namespace BridgeportClaims.FileWatcherBusiness.Providers
{
    public class InvoiceFileWatcherProvider : FileWatcherProvider
    {
        public InvoiceFileWatcherProvider() : base(FileType.Invoices) { }
    }
}