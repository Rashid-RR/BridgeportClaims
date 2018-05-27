using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.KPI
{
    public interface IKpiProvider
    {
        IList<LeftRightClaimsDto> GetClaimComparisons(int leftClaimId, int rightClaimId);
        IList<PaymentTotalsDto> GetPaymentTotalsDtos();
    }
}