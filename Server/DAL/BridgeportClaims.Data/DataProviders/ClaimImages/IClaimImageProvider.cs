using System;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.ClaimImages
{
    public interface IClaimImageProvider
    {
        ClaimImagesDto GetClaimImages(int claimId, string sortColumn, string sortDirection, int pageNumber, int pageSize);
        void UpdateDocumentIndex(int documentId, int claimId, byte documentTypeId, DateTime? rxDate, string rxNumber,
            string invoiceNumber, DateTime? injuryDate, string attorneyName, string indexedByUserId);
    }
}