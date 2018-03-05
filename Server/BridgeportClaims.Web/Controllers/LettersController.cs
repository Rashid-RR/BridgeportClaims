using System;
using NLog;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Web.CustomActionResults;
using BridgeportClaims.Word.Enums;
using BridgeportClaims.Word.FileDriver;
using c = BridgeportClaims.Common.StringConstants.Constants;

namespace BridgeportClaims.Web.Controllers
{
    [RoutePrefix("api/letters")]
    [Authorize(Roles = "User")]
    public class LettersController : BaseApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IWordFileDriver _wordFileDriver;
        private const string DocxContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

        public LettersController(IWordFileDriver wordFileDriver)
        {
            _wordFileDriver = wordFileDriver;
        }


        [HttpPost]
        [Route("download")]
        public async Task<IHttpActionResult> GetImeLetter(int claimId, string letterType)
        {
            try
            {
                if (letterType.ToLower() != "be" && letterType.ToLower() != "pip" && letterType.ToLower() != "ime")
                    ThrowLetterTypeException(letterType);
                return await Task.Run(() =>
                {
                    var type = default(LetterType);
                    switch (letterType.ToLower())
                    {
                        case "be":
                            type = LetterType.BenExhaust;
                            break;
                        case "pip":
                            type = LetterType.PipApp;
                            break;
                        case "ime":
                            type = LetterType.Ime;
                            break;
                        default:
                            ThrowLetterTypeException(letterType);
                            break;
                    }
                    var fullFilePath = _wordFileDriver.GetLetterByType(type);
                    return new FileResult(fullFilePath, c.ImeLetterName, DocxContentType);
                }).ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        private void ThrowLetterTypeException(string letterType)
        {
            throw new Exception($"Error, the only letter types allowed are 'be', 'pip' or 'ime'. You passed in '{letterType}'");
        }
    }
}
