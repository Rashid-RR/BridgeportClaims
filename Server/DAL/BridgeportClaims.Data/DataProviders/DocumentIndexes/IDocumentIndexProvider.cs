using System;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.DocumentIndexes
{
    public interface IDocumentIndexProvider
    {
        IndexedInvoiceDto GetIndexedInvoiceData(string invoiceNumber);
        void DeleteDocumentIndex(int documentId);
        // ReSharper disable once IdentifierTypo
        bool UpsertDocumentIndex(int documentId, int claimId, int documentTypeId, DateTime? rxDate,
            string rxNumber, string invoiceNumber, DateTime? injuryDate, string attorneyName, string indexedByUserId);
        bool InsertInvoiceIndex(int documentId, string invoiceNumber, string userId);
        void InsertCheckIndex(int documentId, string checkNumber, string userId);
    }
}