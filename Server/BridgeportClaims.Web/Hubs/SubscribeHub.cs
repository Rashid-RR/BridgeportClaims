using Microsoft.AspNet.SignalR;

namespace BridgeportClaims.Web.Hubs
{
    public class SubscribeHub : Hub
    {
        public void Subscribe(string customerId)
        {
            Groups.Add(Context.ConnectionId, customerId);
        }

        public void Unsubscribe(string customerId)
        {
            Groups.Remove(Context.ConnectionId, customerId);
        }
    }
}