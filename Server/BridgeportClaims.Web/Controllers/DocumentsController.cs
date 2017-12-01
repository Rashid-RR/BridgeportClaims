using NLog;
using System;
using System.Net;
using System.Web.Http;
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

        public DocumentsController(IDocumentsProvider documentsProvider)
        {
            _documentsProvider = documentsProvider;
        }

        [HttpPost]
        [Route("get")]
        public IHttpActionResult GetDocuments(DocumentViewModel model)
        {
            try
            {
                return Ok(_documentsProvider.GetDocuments(model.Date, model.Sort, model.SortDirection, model.Page, model.PageSize));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
