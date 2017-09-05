using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace BridgeportClaims.Web.Hubs
{
    [HubName("MessageHub")]
    public class MessageHub : Hub
    {
        public void Send(string message)
        {
            Clients.All.broadcastMessage(message);
        }
    }
}