using System;
using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.KPI
{
    public interface IKpiProvider
    {
        IList<LeftRightClaimsDto> GetClaimComparisons(int leftClaimId, int rightClaimId);
        bool SaveClaimMerge(int claimId, int duplicateClaimId, string claimNumber, int patientId,
            DateTime? injuryDate, int? adjustorId, int payorId, int? claimFlex2Id, string personCode);
        IList<PaymentTotalsDto> GetPaymentTotalsDtos();
    }
}