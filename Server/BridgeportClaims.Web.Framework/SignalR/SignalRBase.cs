using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace BridgeportClaims.Web.Framework.SignalR
{
    public abstract class SignalRBase<THub> where THub : IHub
    {
        private readonly Lazy<IHubContext> _hub = new Lazy<IHubContext>(
            () =>
            {
                var hubCtx = GlobalHost.ConnectionManager.GetHubContext<THub>();
                return hubCtx;
            });

        protected IHubContext Hub => _hub.Value;
    }
}