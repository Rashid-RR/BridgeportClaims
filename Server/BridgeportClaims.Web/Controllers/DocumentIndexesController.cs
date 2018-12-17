using NLog;
using System;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.DocumentIndexes;
using BridgeportClaims.Data.DataProviders.Episodes;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Web.Framework.Models;
using BridgeportClaims.Web.Hubs;
using Microsoft.AspNet.Identity;
using g = Microsoft.AspNet.SignalR;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/index-document")]
    public class DocumentIndexesController : BaseApiController
    {
        private readonly Lazy<IDocumentIndexProvider> _documentIndexProvider;
        private readonly Lazy<IEpisodesDataProvider> _episodesDataProvider;
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);

        public DocumentIndexesController(Lazy<IDocumentIndexProvider> documentIndexProvider,
            Lazy<IEpisodesDataProvider> episodesDataProvider)
        {
            _documentIndexProvider = documentIndexProvider;
            _episodesDataProvider = episodesDataProvider;
        }

        [HttpPost]
        [Route("inv-num-exists")]
        public IHttpActionResult InvoiceNumberExists(string invoiceNumber)
        {
            try
            {
                var indexedInvoiceData = _documentIndexProvider.Value.GetIndexedInvoiceData(invoiceNumber) ?? new IndexedInvoiceDto();
                if (indexedInvoiceData.FileUrl.IsNullOrWhiteSpace())
                    indexedInvoiceData.InvoiceNumberIsAlreadyIndexed = false;
                return Ok(indexedInvoiceData);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
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
                _documentIndexProvider.Value.InsertInvoiceIndex(documentId, invoiceNumber, userId);
                const string msg = "The invoice was indexed successfully.";
                return Ok(new {message = msg});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        [HttpPost]
        [Route("index-check")]
        public IHttpActionResult IndexCheckDocument(int documentId, string checkNumber)
        {
            try
            {
                const int i = default(int);
                if (i == documentId)
                {
                    throw new Exception($"Invalid document Id: {i}");
                }
                if (checkNumber.IsNullOrWhiteSpace())
                {
                    throw new Exception("Cannot have a null or empty or white space checkNumber.");
                }
                var userId = User.Identity.GetUserId();
                _documentIndexProvider.Value.InsertCheckIndex(documentId, checkNumber, userId);
                var msg = $"The check # {checkNumber} was indexed successfully.";
                return Ok(new {message = msg});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        [HttpPost]
        [Route("save")]
        public IHttpActionResult SaveDocumentIndex(DocumentIndexViewModel model)
        {
            try
            {
                if (null == model)
                    throw new ArgumentNullException(nameof(model));
                var userId = User.Identity.GetUserId();
                var wasUpdate = _documentIndexProvider.Value.UpsertDocumentIndex(model.DocumentId, model.ClaimId,
                    model.DocumentTypeId, model.RxDate.ToNullableFormattedDateTime(), model.RxNumber,
                    model.InvoiceNumber, model.InjuryDate.ToNullableFormattedDateTime(), model.AttorneyName,
                    userId);
                if (model.DocumentId != default(int))
                {
                    var hubContext = g.GlobalHost.ConnectionManager.GetHubContext<DocumentsHub>();
                    hubContext.Clients.All.indexedDocument(model.DocumentId);
                }

                var episodeCreated = _episodesDataProvider.Value.CreateImageCategoryEpisode(model.DocumentTypeId,
                    model.ClaimId, model.RxNumber, userId, model.DocumentId);
                var msg = $"The image was {(wasUpdate ? "re-indexed" : "indexed")} successfully.";
                if (episodeCreated)
                    msg += " And a new episode was created.";
                if (!cs.AppIsInDebugMode) return Ok(new {message = msg});
                if (episodeCreated)
                    Logger.Value.Info("An episode was created.");
                Logger.Value.Info($"Document ID: {model.DocumentId}. {msg}");
                return Ok(new {message = msg});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }
    }
}