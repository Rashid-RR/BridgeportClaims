using System.Collections.Generic;
using System.Threading.Tasks;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.ClaimsUserHistories
{
    public interface IClaimsUserHistoryProvider
    {
        Task<IList<ClaimsUserHistoryDto>> GetClaimsUserHistoryAsync(string userId);
        Task InsertClaimsUserHistoryAsync(string userId, int claimId);
    }
}