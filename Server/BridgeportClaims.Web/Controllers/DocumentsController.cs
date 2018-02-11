using NLog;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.ClaimSearches;
using BridgeportClaims.Data.DataProviders.Documents;
using BridgeportClaims.Web.Hubs;
using BridgeportClaims.Web.Models;
using Microsoft.AspNet.Identity;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "Indexer, Admin")]
    [RoutePrefix("api/document")]
    public class DocumentsController : BaseApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IDocumentsProvider _documentsProvider;
        private readonly IClaimSearchProvider _claimSearchProvider;

        public DocumentsController(IDocumentsProvider documentsProvider, IClaimSearchProvider claimSearchProvider)
        {
            _documentsProvider = documentsProvider;
            _claimSearchProvider = claimSearchProvider;
        }

        [HttpPost]
        [Route("archive/{documentId}")]
        public async Task<IHttpActionResult> ArchiveDocument(int documentId)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var userId = User.Identity.GetUserId();
                    // Database call
                    _documentsProvider.ArchiveDocument(documentId, userId);
                    // SignalR Call
                    var hubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<DocumentsHub>();
                    hubContext.Clients.All.archivedDocument(documentId);
                    return Ok(new {message = "The document was archived successfully."});
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("claim-search")]
        public IHttpActionResult GetClaimResult(ClaimSearchViewModel model)
        {
            try
            {
                return Ok(_claimSearchProvider.GetDocumentClaimSearchResults(model.SearchText, model.ExactMatch, model.Delimiter));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("get")]
        public async Task<IHttpActionResult> GetDocuments(DocumentViewModel model)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var results = _documentsProvider.GetDocuments(model.Date.ToNullableFormattedDateTime(), model.Archived, model.FileName, model.Sort,
                        model.SortDirection, model.Page,
                        model.PageSize);
                    return Ok(results);
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
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
                    , model.CreationTimeLocal.ToFormattedDateTime(), model.LastAccessTimeLocal.ToFormattedDateTime(), 
                    model.LastWriteTimeLocal.ToFormattedDateTime(), model.FullFilePath, model.FileUrl);
                return Ok(new {message = "A new document has just been added."});
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
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
                    , model.CreationTimeLocal.ToFormattedDateTime(), model.LastAccessTimeLocal.ToFormattedDateTime(),
                    model.LastWriteTimeLocal.ToFormattedDateTime(), model.FullFilePath, model.FileUrl);
                return Ok(new {message = "A document has just been modified."});
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
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
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
