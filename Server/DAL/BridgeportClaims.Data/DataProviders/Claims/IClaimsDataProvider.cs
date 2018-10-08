using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Data.Enums;

namespace BridgeportClaims.Data.DataProviders.Claims
{
    public interface IClaimsDataProvider
    {
        IList<GetClaimsSearchResults> GetClaimsData(string claimNumber, string firstName, string lastName, string rxNumber, string invoiceNumber);
        IList<PrescriptionDto> GetPrescriptionDataByClaim(int claimId, string sort, string direction, int page, int pageSize);
        EntityOperation AddOrUpdateFlex2(int claimId, int claimFlex2Id, string modifiedByUserId);
        IList<EpisodeBladeDto> GetEpisodesBlade(int claimId, string sortColumn, string sortDirection, string userId);
        ClaimDto GetClaimsDataByClaimId(int claimId, string userId);
        BillingStatementDto GetBillingStatementDto(int claimId);
        string UpdateIsMaxBalance(int claimId, bool isMaxBalance, string modifiedByUserId);
    }
}