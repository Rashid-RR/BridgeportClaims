using System;
using NLog;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Web.CustomActionResults;
using BridgeportClaims.Word.Enums;
using BridgeportClaims.Word.FileDriver;
using Microsoft.AspNet.Identity;
using s = BridgeportClaims.Common.Constants.StringConstants;

namespace BridgeportClaims.Web.Controllers
{
    [RoutePrefix("api/letters")]
    [Authorize(Roles = "User")]
    public class LettersController : BaseApiController
    {
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);
        private readonly Lazy<IWordFileDriver> _wordFileDriver;
        private const string DocxContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

        public LettersController(Lazy<IWordFileDriver> wordFileDriver)
        {
            _wordFileDriver = wordFileDriver;
        }


        [HttpPost]
        [Route("download")]
        public IHttpActionResult GetLetter(int claimId, string letterType, int? prescriptionId = null)
        {
            try
            {
                if (letterType.ToLower() != "be" && letterType.ToLower() != "pip" && letterType.ToLower() != "ime" &&
                    letterType.ToLower() != "den" && letterType.ToLower() != "ui")
                {
                    ThrowLetterTypeException(letterType);
                }
                var userId = User.Identity.GetUserId();
                if (userId.IsNullOrWhiteSpace())
                {
                    throw new ArgumentNullException(nameof(userId));
                }
                var type = default(LetterType);
                var fileName = string.Empty;
                switch (letterType.ToLower())
                {
                    case "be":
                        type = LetterType.BenExhaust;
                        fileName = s.BenefitsExhaustedLetter;
                        break;
                    case "pip":
                        type = LetterType.PipApp;
                        fileName = s.PipAppLetter;
                        break;
                    case "ime":
                        type = LetterType.Ime;
                        fileName = s.ImeLetterName;
                        break;
                    case "den":
                        type = LetterType.Denial;
                        fileName = s.DenialLetterName;
                        break;
                    case "ui":
                        type = LetterType.UnderInvestigation;
                        fileName = s.UnderInvestigationLetterName;
                        break;
                    default:
                        ThrowLetterTypeException(letterType);
                        break;
                }
                var fullFilePath = _wordFileDriver.Value.GetLetterByType(claimId, userId, type, prescriptionId);
                return new FileResult(fullFilePath, fileName, DocxContentType);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        private static void ThrowLetterTypeException(string letterType) => throw new Exception(
            $"Error, the only letter types allowed are 'be', 'pip', 'den', 'ui' or 'ime'. You passed in '{letterType}'");
    }
}
