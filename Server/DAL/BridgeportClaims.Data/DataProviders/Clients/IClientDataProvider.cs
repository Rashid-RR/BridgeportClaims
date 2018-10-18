using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.Clients
{
    public interface IClientDataProvider
    {
        void InsertReferral(ReferralDto referral);
    }
}