using System;

namespace BridgeportClaims.Data.DataProviders.DocumentIndexes
{
    public interface IDocumentIndexProvider
    {
        void DeleteDocumentIndex(int documentId);
        bool UpsertDocumentIndex(int documentId, int claimId, int documentTypeId, DateTime? rxDate,
            string rxNumber, string invoiceNumber, DateTime? injuryDate, string attorneyName, string indexedByUserId);
        void InsertInvoiceIndex(int documentId, string invoiceNumber, string userId);
    }
}