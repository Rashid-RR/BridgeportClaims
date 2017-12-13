using NLog;
using System;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.ClaimSearches;
using BridgeportClaims.Data.DataProviders.Documents;
using BridgeportClaims.Web.Models;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
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
        public IHttpActionResult GetClaimResult(string searchText)
        {
            try
            {
                return Ok(_claimSearchProvider.GetDocumentClaimSearchResults(searchText));
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
    }
}
