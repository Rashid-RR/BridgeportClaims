using System.Collections.Generic;
using PdfGeneratorApi.Common.Models;
using PdfGeneratorApi.Data.Dtos;

namespace PdfGeneratorApi.Data.DataProviders.InvoicePdfDocuments
{
    public interface IInvoicePdfDocumentProvider
    {
        IEnumerable<InvoicePdfDto> GetInvoicePdfDocument(string userId);
        InvoicePdfModel GetInvoicePdfModel(IList<InvoicePdfDto> data);
    }
}