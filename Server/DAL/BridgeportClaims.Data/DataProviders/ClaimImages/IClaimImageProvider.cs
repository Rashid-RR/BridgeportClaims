using System;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.ClaimImages
{
    public interface IClaimImageProvider
    {
        ClaimImagesDto GetClaimImages(int claimId, string sortColumn, string sortDirection, int pageNumber, int pageSize);
        void UpdateDocumentIndex(int documentId, DateTime? rxDate, string rxNumber, byte documentTypeId);
    }
}