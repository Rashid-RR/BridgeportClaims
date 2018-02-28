using NLog;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.Claims;
using BridgeportClaims.Data.DataProviders.UserOptions;
using BridgeportClaims.Web.CustomActionResults;
using BridgeportClaims.Web.Email.EmailModelGeneration;
using BridgeportClaims.Web.Email.EmailTemplateProviders;
using BridgeportClaims.Web.EmailTemplates;
using BridgeportClaims.Word.Factories;
using BridgeportClaims.Word.WordProvider;
using c = BridgeportClaims.Common.StringConstants.Constants;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Web.Controllers
{
    [RoutePrefix("api/test")]
    public class TestChecksController : BaseApiController
    {
        private readonly IDbccUserOptionsProvider _dbccUserOptionsProvider;
        private readonly IClaimsDataProvider _claimsDataProvider;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IEmailService _emailService;

        public TestChecksController(IDbccUserOptionsProvider dbccUserOptionsProvider, 
            IClaimsDataProvider claimsDataProvider, IEmailService emailService)
        {
            _dbccUserOptionsProvider = dbccUserOptionsProvider;
            _claimsDataProvider = claimsDataProvider;
            _emailService = emailService;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("download")]
        public IHttpActionResult Download()
        {
            try
            {
                var wordDocumentProvider = new WordDocumentProvider();
                var path = wordDocumentProvider.GetWordDocument();
                return new FileResult(path, "IME Letter.docx", "application/msword");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("call")]
        public async Task<IHttpActionResult> InitializeTestCall()
        {
            try
            {
                await _emailService.SendEmail<EmailTemplateProvider>("jordangurney@gmail.com", "Test Message",
                    string.Empty,
                    EmailModelEnum.LakerImportStatus);
                var testClaimId = Convert.ToInt32(cs.GetAppSetting(c.TestClaimIdKey));
                _dbccUserOptionsProvider.IsSessionUsingReadCommittedSnapshotIsolation();
                var retVal = _claimsDataProvider.GetClaimsDataByClaimId(testClaimId, Guid.NewGuid().ToString());
                return Ok(retVal);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
