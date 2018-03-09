using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.DocumentIndexes;
using BridgeportClaims.Data.DataProviders.Episodes;
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
        private readonly IEpisodesDataProvider _episodesDataProvider;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public DocumentIndexesController(IDocumentIndexProvider documentIndexProvider,
            IEpisodesDataProvider episodesDataProvider)
        {
            _documentIndexProvider = documentIndexProvider;
            _episodesDataProvider = episodesDataProvider;
        }

        [HttpPost]
        [Route("index-invoice")]
        public IHttpActionResult IndexInvoiceDocument(int documentId, string invoiceNumber)
        {
            try
            {
                if (0 == documentId)
                    throw new Exception("Invalid document Id");
                var userId = User.Identity.GetUserId();
                _documentIndexProvider.InsertInvoiceIndex(documentId, invoiceNumber, userId);
                const string msg = "The invoice was indexed successfully.";
                return Ok(new {message = msg});
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
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
                    var wasUpdate = _documentIndexProvider.UpsertDocumentIndex(model.DocumentId, model.ClaimId,
                        model.DocumentTypeId, model.RxDate.ToNullableFormattedDateTime(), model.RxNumber,
                        model.InvoiceNumber, model.InjuryDate.ToNullableFormattedDateTime(), model.AttorneyName,
                        userId);
                    if (model.DocumentId != default(int))
                    {
                        var hubContext = GlobalHost.ConnectionManager.GetHubContext<DocumentsHub>();
                        hubContext.Clients.All.indexedDocument(model.DocumentId);
                    }

                    var episodeCreated = _episodesDataProvider.CreateImageCategoryEpisode(model.DocumentTypeId,
                        model.ClaimId, model.RxNumber, userId, model.DocumentId);
                    var msg = $"The image was {(wasUpdate ? "reindexed" : "indexed")} successfully.";
                    if (episodeCreated)
                        msg += " And a new episode was created.";
                    if (!cs.AppIsInDebugMode) return Ok(new {message = msg});
                    if (episodeCreated)
                        Logger.Info("An episode was created.");
                    Logger.Info($"Document ID: {model.DocumentId}. {msg}");
                    return Ok(new {message = msg});
                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }
    }
}