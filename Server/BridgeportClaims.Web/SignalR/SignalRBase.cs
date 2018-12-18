using System;
using BridgeportClaims.Web.Controllers;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace BridgeportClaims.Web.SignalR
{
    public abstract class SignalRBase<THub> : BaseApiController where THub : IHub
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