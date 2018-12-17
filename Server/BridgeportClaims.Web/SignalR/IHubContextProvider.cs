using Microsoft.AspNet.SignalR;

namespace BridgeportClaims.Web.SignalR
{
    public interface IHubContextProvider<THubClient> where THubClient : class
    {
        IHubContext<THubClient> GetHubContext<THub>() where THub : Hub<THubClient>;
    }
}