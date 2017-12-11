using System;
using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.Documents
{
    public interface IDocumentsProvider
    {
        IList<DocumentTypeDto> GetDocumentTypes();
        DocumentsDto GetDocuments(bool isIndexed, DateTime? date, string sortColumn, string sortDirection, int pageNumber, int pageSize);
    }
}