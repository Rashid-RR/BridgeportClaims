using System;

namespace BridgeportClaims.Data.DataProviders.DocumentIndexes
{
    public interface IDocumentIndexProvider
    {
        string InvoiceNumberExists(string invoiceNumber);
        void DeleteDocumentIndex(int documentId);
        bool UpsertDocumentIndex(int documentId, int claimId, int documentTypeId, DateTime? rxDate,
            string rxNumber, string invoiceNumber, DateTime? injuryDate, string attorneyName, string indexedByUserId);
        bool InsertInvoiceIndex(int documentId, string invoiceNumber, string userId);
    }
}