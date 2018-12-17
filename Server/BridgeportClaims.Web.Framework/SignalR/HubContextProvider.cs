using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;

namespace BridgeportClaims.Web.Framework.SignalR
{
    public class HubContextProvider<THubClient> : IHubContextProvider<THubClient>
        where THubClient : class
    {
        private readonly IConnectionManager _connectionManager;

        public HubContextProvider(ConnectionConfiguration hubConfiguration) => _connectionManager = hubConfiguration.Resolver.Resolve<IConnectionManager>();

        public IHubContext<THubClient> GetHubContext<THub>()
                where THub : Hub<THubClient> => _connectionManager.GetHubContext<THub, THubClient>();
    }
}