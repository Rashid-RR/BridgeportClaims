using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.ClaimImages
{
    public interface IClaimImageProvider
    {
        ClaimImagesDto GetClaimImages(string sortColumn, string sortDirection, int pageNumber, int pageSize);
    }
}