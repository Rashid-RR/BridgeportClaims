using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.ClaimsUserHistories
{
    public interface IClaimsUserHistoryProvider
    {
        IList<ClaimsUserHistoryDto> GetClaimsUserHistory(string userId);
        void InsertClaimsUserHistory(string userId, int claimId);
    }
}