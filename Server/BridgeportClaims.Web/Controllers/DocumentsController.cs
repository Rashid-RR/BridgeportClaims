using NLog;
using System;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.ClaimSearches;
using BridgeportClaims.Data.DataProviders.Documents;
using ic = BridgeportClaims.Common.Constants.IntegerConstants;
using BridgeportClaims.Web.Hubs;
using BridgeportClaims.Web.Models;
using Microsoft.AspNet.Identity;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "Indexer, Admin")]
    [RoutePrefix("api/document")]
    public class DocumentsController : BaseApiController
    {
        private static readonly Lazy<Logger> Logger = new Lazy<Logger>(LogManager.GetCurrentClassLogger);
        private readonly Lazy<IDocumentsProvider> _documentsProvider;
        private readonly Lazy<IClaimSearchProvider> _claimSearchProvider;

        public DocumentsController(Lazy<IDocumentsProvider> documentsProvider, Lazy<IClaimSearchProvider> claimSearchProvider)
        {
            _documentsProvider = documentsProvider;
            _claimSearchProvider = claimSearchProvider;
        }

        [HttpPost]
        [Route("archive/{documentId}")]
        public IHttpActionResult ArchiveDocument(int documentId)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                // Database call
                _documentsProvider.Value.ArchiveDocument(documentId, userId);
                // SignalR Call
                var hubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager
                    .GetHubContext<DocumentsHub>();
                hubContext.Clients.All.archivedDocument(documentId);
                return Ok(new {message = "The document was archived successfully."});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        [HttpPost]
        [Route("claim-search")]
        public IHttpActionResult GetClaimResult(ClaimSearchViewModel model)
        {
            try
            {
                return Ok(_claimSearchProvider.Value.GetDocumentClaimSearchResults(model.SearchText, model.ExactMatch, model.Delimiter));
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("get-invalid")]
        public IHttpActionResult GetInvalidDocuments(InvalidDocumentViewModel model)
        {
            try
            {
                var results = _documentsProvider.Value.GetInvalidDocuments(model.Date.ToNullableFormattedDateTime(),
                    model.FileName, model.Sort, model.SortDirection, model.Page, model.PageSize);
                return Ok(results);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("get")]
        public IHttpActionResult GetDocuments(DocumentViewModel model)
        {
            try
            {
                var results = _documentsProvider.Value.GetDocuments(model.Date.ToNullableFormattedDateTime(),
                    model.Archived, model.FileName, model.FileTypeId, model.Sort,
                    model.SortDirection, model.Page,
                    model.PageSize);
                return Ok(results);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        [HttpPost]
        [Route("add")]
        public IHttpActionResult AddSignalRDocument(DocumentModel model)
        {
            try
            {
                var hubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<DocumentsHub>();
                hubContext.Clients.All.newDocument(model.DocumentId, model.FileName, model.Extension, model.FileSize
                    , model.CreationTimeLocal, model.LastAccessTimeLocal, 
                    model.LastWriteTimeLocal, model.FullFilePath, model.FileUrl);
                return Ok(new {message = "A new document has just been added."});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("edit")]
        public IHttpActionResult EditSignalRDocument(DocumentModel model)
        {
            try
            {
                var hubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<DocumentsHub>();
                hubContext.Clients.All.modifiedDocument(model.DocumentId, model.FileName, model.Extension, model.FileSize
                    , model.CreationTimeLocal, model.LastAccessTimeLocal,
                    model.LastWriteTimeLocal, model.FullFilePath, model.FileUrl);
                return Ok(new {message = "A document has just been modified."});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        [HttpPost]
        [Route("delete")]
        public IHttpActionResult DeleteSignalRDocument(int documentId)
        {
            try
            {
                var hubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<DocumentsHub>();
                hubContext.Clients.All.deletedDocument(documentId);
                return Ok(new { message = "A document has just been removed." });
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
