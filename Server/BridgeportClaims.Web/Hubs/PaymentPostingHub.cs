using Microsoft.AspNet.SignalR;

namespace BridgeportClaims.Web.Hubs
{
    public class PaymentPostingHub : Hub
    {
        public void GetAllPostedPayments()
        {
            Clients.Caller.setAmountRemaining(999.99);
        }
    }
}