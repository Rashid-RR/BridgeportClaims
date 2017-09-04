using System;
using Microsoft.AspNet.SignalR;

namespace BridgeportClaims.Web.Hubs
{
    public class ClockHub : Hub
    {
        public void GetRealTime()
        {
            Clients.Caller.setRealTime(DateTime.Now.ToLocalTime().ToString("h:mm:ss tt"));
        }
    }
}