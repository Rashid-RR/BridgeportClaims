using BridgeportClaims.Common.Caching;
using BridgeportClaims.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using c = BridgeportClaims.Common.StringConstants.Constants;

namespace BridgeportClaims.Web.Hubs
{
    public class PaymentPostingHub : Hub
    {
        private readonly IMemoryCacher _memoryCacher;

        public PaymentPostingHub(IMemoryCacher memoryCacher)
        {
            _memoryCacher = memoryCacher;
        }

        public void GetAllPostedPayments()
        {
            var userPaymentPostingCacheKey =
                _memoryCacher.GetPaymentPostingCacheKey(Context.User.Identity.GetUserName());
            var model = _memoryCacher.GetItem(userPaymentPostingCacheKey) as UserPaymentPostingSession;
            if (null != model)
            {
                Clients.Client(model.DestinationConnectionId).postPayment(model.PrescriptionPosts);
            }
        }
    }
}