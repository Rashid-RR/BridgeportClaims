using System;
using Microsoft.AspNet.SignalR;

namespace BridgeportClaims.Web.SignalR
{
    public class SignalRChatHub : Hub
    {
        public void BroadCastMessage(String msgFrom, String msg)
        {
            Clients.All.receiveMessage(msgFrom, msg);
        }
    }
}