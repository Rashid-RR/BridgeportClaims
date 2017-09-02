using System.Threading.Tasks;
using BridgeportClaims.Web.SignalR;
using Microsoft.AspNet.SignalR;
using c = BridgeportClaims.Common.StringConstants.Constants;

namespace BridgeportClaims.Web.Hubs
{
    public class PaymentHub : Hub
    {
        public async Task Subscribe(string channel)
        {
            await Groups.Add(Context.ConnectionId, channel);
            var evnt = new ChannelEvent
            {
                ChannelName = c.PaymentChannel,
                Name = "user.subscribed",
                Data = new
                {
                    Context.ConnectionId,
                    ChannelName = channel
                }
            };
            await Publish(evnt);
        }

        public async Task Unsubscribe(string channel)
        {
            await Groups.Remove(Context.ConnectionId, channel);

            var ev = new ChannelEvent
            {
                ChannelName = c.PaymentChannel,
                Name = "user.unsubscribed",
                Data = new
                {
                    Context.ConnectionId,
                    ChannelName = channel
                }
            };
            await Publish(ev);
        }


        public Task Publish(ChannelEvent channelEvent)
        {
            Clients.Group(channelEvent.ChannelName).OnEvent(channelEvent.ChannelName, channelEvent);
            if (channelEvent.ChannelName != c.PaymentChannel)
            {
                // Push this out on the payment channel
                //
                Clients.Group(c.PaymentChannel).OnEvent(c.PaymentChannel, channelEvent);
            }
            return Task.FromResult(0);
        }


        public override Task OnConnected()
        {
            var ev = new ChannelEvent
            {
                ChannelName = c.PaymentChannel,
                Name = "user.connected",
                Data = new { Context.ConnectionId }
            };
            Publish(ev);
            return base.OnConnected();
        }


        public override Task OnDisconnected(bool stopCalled)
        {
            var ev = new ChannelEvent
            {
                ChannelName = c.PaymentChannel,
                Name = "user.disconnected",
                Data = new { Context.ConnectionId }
            };
            Publish(ev);
            return base.OnDisconnected(stopCalled);
        }
    }
}