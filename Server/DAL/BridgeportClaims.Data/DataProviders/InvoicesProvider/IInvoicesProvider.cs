using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.InvoicesProvider
{
    public interface IInvoicesProvider
    {
        IEnumerable<InvoiceDto> GetInvoices();
        IEnumerable<InvoiceProcessDto> GetInvoiceProcesses();
    }
}