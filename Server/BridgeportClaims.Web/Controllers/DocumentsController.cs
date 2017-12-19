using NLog;
using System;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.ClaimSearches;
using BridgeportClaims.Data.DataProviders.Documents;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Web.Hubs;
using BridgeportClaims.Web.Models;
using Microsoft.AspNet.SignalR;

namespace BridgeportClaims.Web.Controllers
{
    [System.Web.Http.Authorize(Roles = "User")]
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
        public IHttpActionResult GetDocuments(DocumentViewModel model)
        {
            try
            {
                var results = _documentsProvider.GetDocuments(model.Date, model.Sort, 
                    model.SortDirection, model.Page,
                    model.PageSize);
                return Ok(results);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("add")]
        public IHttpActionResult AddSignalRDocument(DocumentResultDto dto)
        {
            try
            {
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<DocumentsHub>();
                hubContext.Clients.All.newDocument(dto.DocumentId, dto.FileName, dto.Extension, dto.FileSize
                    , dto.CreationTimeLocal, dto.LastAccessTimeLocal, dto.LastWriteTimeLocal, dto.FullFilePath, dto.FileUrl);
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("edit")]
        public IHttpActionResult EditSignalRDocument(DocumentResultDto dto)
        {
            try
            {
                /* TODO: see if anything that we care about in the document has actually changed, before calling hub method. */
                if (true) // TODO: conditional upon whether the document change merits an update to the Unindexed Images page.
                {
                    var hubContext = GlobalHost.ConnectionManager.GetHubContext<DocumentsHub>();
                    hubContext.Clients.All.modifiedDocument(dto.DocumentId, dto.FileName, dto.Extension, dto.FileSize
                        , dto.CreationTimeLocal, dto.LastAccessTimeLocal, dto.LastWriteTimeLocal, dto.FullFilePath,
                        dto.FileUrl);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        public IHttpActionResult DeleteSignalRDocument(int documentId)
        {
            try
            {
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<DocumentsHub>();
                hubContext.Clients.All.deletedDocument(documentId);
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
