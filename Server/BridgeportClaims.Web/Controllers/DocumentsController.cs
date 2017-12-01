using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.Documents;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/document")]
    public class DocumentsController : BaseApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly DocumentsProvider _documentsProvider;

        [HttpPost]
        [Route("remove")]
        public IHttpActionResult RemoveDiary(int prescriptionNoteId)
        {
            try
            {
                _diaryProvider.RemoveDiary(prescriptionNoteId);
                return Ok(new { message = "The diary entry was removed successfully." });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
