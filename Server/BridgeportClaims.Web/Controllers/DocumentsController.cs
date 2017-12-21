using NLog;
using System;
using System.Net;
using System.Threading.Tasks;
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
        [Route("get-by-filename")]
        public async Task<IHttpActionResult> GetDocumentByFileName(string fileName)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var doc = _documentsProvider.GetDocumentByFileName(fileName);
                    return Ok(doc);
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
        public IHttpActionResult EditSignalRDocument(DocumentResultDto dto)
        {
            try
            {
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<DocumentsHub>();
                hubContext.Clients.All.modifiedDocument(dto.DocumentId, dto.FileName, dto.Extension, dto.FileSize
                    , dto.CreationTimeLocal, dto.LastAccessTimeLocal, dto.LastWriteTimeLocal, dto.FullFilePath,
                    dto.FileUrl);
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
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<DocumentsHub>();
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
