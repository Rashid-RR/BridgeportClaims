﻿using System.Collections.Generic;
using BridgeportClaims.Common.Models;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.InvoicePdfDocuments
{
    public interface IInvoicePdfDocumentProvider
    {
        IEnumerable<InvoicePdfDto> GetInvoicePdfDocument(string userId);
        InvoicePdfModel GetInvoicePdfModel(IList<InvoicePdfDto> data);
    }
}