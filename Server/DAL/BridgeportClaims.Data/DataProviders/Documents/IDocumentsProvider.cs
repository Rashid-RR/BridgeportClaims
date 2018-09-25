using System;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.Documents
{
    public interface IDocumentsProvider
    {
        void ArchiveDocument(int documentId, string modifiedByUserId);
        DocumentsDto GetDocuments(DateTime? date, bool archived, string fileName, 
            int fileTypeId, string sortColumn, string sortDirection, int pageNumber, int pageSize);
        DocumentsDto GetInvalidDocuments(DateTime? date, bool archived, string fileName, int fileTypeId,
            string sortColumn, string sortDirection, int pageNumber, int pageSize);
    }
}