using Microsoft.AspNet.SignalR;

namespace BridgeportClaims.Web.Hubs
{
    public class DocumentsHub : Hub
    {
        public void BroadCastMessage(string msgFrom, string msg)
        {
            Clients.All.receiveMessage(msgFrom, msg);
        }
    }
}