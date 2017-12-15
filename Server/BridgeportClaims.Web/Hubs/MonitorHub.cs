using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace BridgeportClaims.Web.Hubs
{
    [HubName("MonitorHub")]
    public class MonitorHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}