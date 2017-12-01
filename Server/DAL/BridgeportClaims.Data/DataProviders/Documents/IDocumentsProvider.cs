using System;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.Documents
{
    public interface IDocumentsProvider
    {
        DocumentsDto GetDocuments(DateTime? date, string sortColumn, string sortDirection, int pageNumber, int pageSize);
    }
}