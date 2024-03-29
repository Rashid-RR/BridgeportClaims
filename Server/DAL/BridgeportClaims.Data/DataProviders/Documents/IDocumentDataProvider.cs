using System;
using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.Documents
{
    public interface IDocumentDataProvider
    {
        void ArchiveDocument(int documentId, string modifiedByUserId);
        DocumentsDto GetDocuments(DateTime? date, bool archived, string fileName, 
            int fileTypeId, string sortColumn, string sortDirection, int pageNumber, int pageSize);
        DocumentsDto GetInvalidDocuments(DateTime? date, string fileName, string sortColumn, string sortDirection,
            int pageNumber, int pageSize);
        IndexedChecksDto GetIndexedChecks(DateTime? date, string fileName, string sortColumn, string sortDirection,
            int pageNumber, int pageSize);
        IEnumerable<IndexedChecksDetailDto> GetIndexedCheckDetails(int documentId);
        void ReIndexCheck(int documentId, bool skipPayments, int? prescriptionPaymentId);
    }
}