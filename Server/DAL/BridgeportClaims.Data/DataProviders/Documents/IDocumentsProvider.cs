using System;
using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.Documents
{
    public interface IDocumentsProvider
    {
        IList<DocumentResultDto> GetDocumentByFileName(string fileName);
        IList<DocumentTypeDto> GetDocumentTypes();
        void ArchiveDocument(int documentId, string modifiedByUserId);
        DocumentsDto GetDocuments(DateTime? date, bool archived, string fileName, string sortColumn, string sortDirection, int pageNumber, int pageSize);
    }
}