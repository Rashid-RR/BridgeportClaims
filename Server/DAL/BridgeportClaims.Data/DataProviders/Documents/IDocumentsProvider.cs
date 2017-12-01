using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.Documents
{
    public interface IDocumentsProvider
    {
        DocumentsDto GetDocuments(string sortColumn, string sortDirection, int pageNumber, int pageSize);
    }
}