using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.ClaimImages
{
    public interface IClaimImageProvider
    {
        ClaimImagesDto GetClaimImages(int claimId, string sortColumn, string sortDirection, int pageNumber, int pageSize);
    }
}