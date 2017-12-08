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
            Clients.All.receiveMessage(msgFrom, msg);
        }

        public DocumentsDto GetDocuments(DocumentViewModel model)
        {
            return _documentsService.GetDocuments(model);
        }
    }
}