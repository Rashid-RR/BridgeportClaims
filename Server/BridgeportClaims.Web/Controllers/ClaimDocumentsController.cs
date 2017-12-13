using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.Documents;
using BridgeportClaims.Web.Models;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/claim-documents")]
    public class ClaimDocumentsController : ApiController
    {
        private readonly IDocumentsProvider _documentsProvider;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ClaimDocumentsController(IDocumentsProvider documentsProvider)
        {
            _documentsProvider = documentsProvider;
        }

        [HttpPost]
        [Route("get-claim-documents")]
        public async Task<IHttpActionResult> GetClaimDocuments(ClaimDocumentViewModel model)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var result = _documentsProvider.GetDocuments(true, null, model.Sort, model.SortDirection, model.Page, model.PageSize);
                    result.ClaimId = model.ClaimId;
                    result.DocumentTypes = null;
                    return Ok(result);
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
