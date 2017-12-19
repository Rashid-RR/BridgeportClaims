using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace BridgeportClaims.Web.Hubs
{
    [HubName("documentsHub")]
    public class DocumentsHub : Hub 
    {
        public void BroadCastMessage(string msgFrom, string msg)
        {
            Clients.All.receiveMessage(msgFrom, msg);
        }

        #region Documents Methods

        public void SendNewDocument(int documentId, string fileName, string fileSize, DateTime created, DateTime lastAccess, DateTime lastWrite)
        {
            Clients.All.newDocument(documentId, fileName, fileSize, created, lastAccess, lastWrite);
        }

        #endregion
    }
}