using System.Threading.Tasks;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Web.Models;
using BridgeportClaims.Web.SignalR;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace BridgeportClaims.Web.Hubs
{
    [HubName("DocumentsHub")]
    public class DocumentsHub : Hub
    {
        private readonly DocumentsService _documentsService;

        #region Ctors
        
        public DocumentsHub(DocumentsService documentsService)
        {
            _documentsService = documentsService;
        }

        public DocumentsHub() : this(DocumentsService.Instance) { }

        #endregion

        public void BroadCastMessage(string msgFrom, string msg)
        {
            var message = $"{Context.ConnectionId}: {msg}";
            Clients.All.receiveMessage(msgFrom, message);
        }

        public void JoinRoom(string room)
        {
            // NOTE: this is not persisted...
            Groups.Add(Context.ConnectionId, room);
        }

        public void SendMessageToRoom(string room, string message)
        {
            Clients.Group(room).newMessage(message);
        }

        public void BroadCastExternalMessage(string msgFrom, string msg)
        {
            var ctx = GlobalHost.ConnectionManager.GetHubContext<DocumentsHub>();
            ctx.Clients.All.receiveMessage(msgFrom, msg);
        }

        public DocumentsDto GetDocuments(DocumentViewModel model)
        {
            return _documentsService.GetDocuments(model);
        }


        #region Overrides

        public override Task OnConnected()
        {

        }

        public override Task OnDisconected()
        {

        }

        #endregion
    }
}