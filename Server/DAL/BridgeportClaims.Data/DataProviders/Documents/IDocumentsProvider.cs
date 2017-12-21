using System;
using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.Documents
{
    public interface IDocumentsProvider
    {
        IList<DocumentResultDto> GetDocumentByFileName(string fileName);
        IList<DocumentTypeDto> GetDocumentTypes();
        DocumentsDto GetDocuments(DateTime? date, string fileName, string sortColumn, string sortDirection, int pageNumber, int pageSize);
    }
}