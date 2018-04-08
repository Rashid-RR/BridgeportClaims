using NLog;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.Claims;
using BridgeportClaims.Data.DataProviders.UserOptions;
using BridgeportClaims.Pdf.Factories;
using BridgeportClaims.Web.Email.EmailModelGeneration;
using BridgeportClaims.Web.Email.EmailTemplateProviders;
using BridgeportClaims.Web.EmailTemplates;
using c = BridgeportClaims.Common.StringConstants.Constants;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Web.Controllers
{
    [RoutePrefix("api/test")]
    public class TestChecksController : BaseApiController
    {
        private readonly Lazy<IDbccUserOptionsProvider> _dbccUserOptionsProvider;
        private readonly Lazy<IClaimsDataProvider> _claimsDataProvider;
        private static readonly Lazy<Logger> Logger = new Lazy<Logger>(LogManager.GetCurrentClassLogger);
        private readonly Lazy<IEmailService> _emailService;
        private readonly Lazy<IPdfFactory> _pdfFactory;

        public TestChecksController(
            Lazy<IDbccUserOptionsProvider> dbccUserOptionsProvider, 
            Lazy<IClaimsDataProvider> claimsDataProvider, 
            Lazy<IEmailService> emailService, 
            Lazy<IPdfFactory> pdfFactory)
        {
            _dbccUserOptionsProvider = dbccUserOptionsProvider;
            _claimsDataProvider = claimsDataProvider;
            _emailService = emailService;
            _pdfFactory = pdfFactory;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("call")]
        public async Task<IHttpActionResult> InitializeTestCall()
        {
            try
            {
                _pdfFactory.Value.MergePdfs(
                    new[]
                    {
                        new Uri(
                            "https://invoices.bridgeportclaims.com/2017/07-Jul/20170720/PROGRESSIVE - DE/INV1500_PROGRESSIVE - DE17342692401.pdf",
                            UriKind.Absolute),
                        new Uri(
                            "https://invoices.bridgeportclaims.com/2017/07-Jul/20170720/PROGRESSIVE - DE/INV1500_PROGRESSIVE - DE17182323001.pdf",
                            UriKind.Absolute),
                        new Uri("https://images.bridgeportclaims.com/03-17/20170324/csp201703240039.pdf",
                            UriKind.Absolute)
                    }, @"C:\Development\PDF\cc.pdf");
                await _emailService.Value.SendEmail<EmailTemplateProvider>("jordangurney@gmail.com", "Test Message",
                    string.Empty,
                    EmailModelEnum.LakerImportStatus);
                var testClaimId = Convert.ToInt32(cs.GetAppSetting(c.TestClaimIdKey));
                _dbccUserOptionsProvider.Value.IsSessionUsingReadCommittedSnapshotIsolation();
                var retVal = _claimsDataProvider.Value.GetClaimsDataByClaimId(testClaimId, Guid.NewGuid().ToString());
                return Ok(retVal);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                throw;
            }
        }
    }
}
