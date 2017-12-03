using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.DocumentIndexes;
using BridgeportClaims.Web.Models;
using NLog;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
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
                    var result = _documentIndexProvider.UpsertDocumentIndex(model.DocumentId, model.ClaimId,
                        model.DocumentTypeId, model.RxDate, model.RxNumber,
                        model.InvoiceNumber, model.InjuryDate, model.AttorneyName);
                    var msg = $"The document was {(result ? "updated" : "inserted")} successfully.";
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
