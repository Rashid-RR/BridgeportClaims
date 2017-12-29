using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.DocumentIndexes;
using BridgeportClaims.Web.Hubs;
using BridgeportClaims.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using NLog;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Web.Controllers
{
    [System.Web.Http.Authorize(Roles = "User")]
    [RoutePrefix("api/index-document")]
    public class DocumentIndexesController : BaseApiController
    {
        private readonly IDocumentIndexProvider _documentIndexProvider;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public DocumentIndexesController(IDocumentIndexProvider documentIndexProvider)
        {
            _documentIndexProvider = documentIndexProvider;
        }

        [HttpPost]
        [Route("save")]
        public async Task<IHttpActionResult> SaveDocumentIndex(DocumentIndexViewModel model)
        {
            try
            {
                return await Task.Run(() =>
                {
                    if (null == model)
                        throw new ArgumentNullException(nameof(model));
                    var userId = User.Identity.GetUserId();
                    var result = _documentIndexProvider.UpsertDocumentIndex(model.DocumentId, model.ClaimId,
                        model.DocumentTypeId, model.RxDate, model.RxNumber,
                        model.InvoiceNumber, model.InjuryDate, model.AttorneyName, userId);
                    if (model.DocumentId != default(int))
                    {
                        var hubContext = GlobalHost.ConnectionManager.GetHubContext<DocumentsHub>();
                        hubContext.Clients.All.deletedDocument(model.DocumentId);
                    }
                    var msg = $"The image was {(result ? "reindexed" : "indexed")} successfully.";
                    if (cs.AppIsInDebugMode)
                        Logger.Info($"Document ID: {model.DocumentId}. {msg}");
                    return Ok(new {message = msg});
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
