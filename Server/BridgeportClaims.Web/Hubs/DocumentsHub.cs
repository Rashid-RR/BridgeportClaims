using System;
using System.Threading.Tasks;
using BridgeportClaims.Data.Dtos;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace BridgeportClaims.Web.Hubs
{
    [HubName("DocumentsHub")]
    public class DocumentsHub : Hub
    {
        public void BroadCastMessage(string msgFrom, string msg)
        {
            Clients.All.receiveMessage(msgFrom, msg);
        }

        #region Overrides

        public override Task OnConnected()
        {
            SendMonitoringData("Connected", Context.ConnectionId);
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            SendMonitoringData("Reconnected", Context.ConnectionId);
            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            SendMonitoringData("Disconnected", Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }


        #endregion

        #region Documents Methods

        public void SendNewDocument(int documentId, string fileName, string fileSize, DateTime created, DateTime lastAccess, DateTime lastWrite)
        {
            var ctx = GlobalHost.ConnectionManager.GetHubContext<MonitorHub>();
            ctx.Clients.All.newDocument(documentId, fileName, fileSize, created, lastAccess, lastWrite);
        }

        #endregion

        public void SendMonitoringData(string eventType, string connectionId)
        {
            var ctx = GlobalHost.ConnectionManager.GetHubContext<MonitorHub>();
            ctx.Clients.All.newEvent(eventType, connectionId);
        }
    }
}