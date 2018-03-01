using System;
using NLog;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Web.CustomActionResults;
using BridgeportClaims.Word.WordProvider;
using c = BridgeportClaims.Common.StringConstants.Constants;

namespace BridgeportClaims.Web.Controllers
{
    [RoutePrefix("api/letters")]
    [Authorize(Roles = "User")]
    public class LettersController : BaseApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IWordDocumentProvider _wordDocumentProvider;

        public LettersController(IWordDocumentProvider wordDocumentProvider)
        {
            _wordDocumentProvider = wordDocumentProvider;
        }

        [HttpPost]
        [Route("ime")]
        public async Task<IHttpActionResult> GetImeLetter(int claimId)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var fullFilePath = _wordDocumentProvider.GetWordDocument();
                    return Ok(fullFilePath); //new FileResult(fullFilePath, c.ImeLetterName, "application/msword");
                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
