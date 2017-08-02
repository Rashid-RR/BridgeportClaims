using NLog;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.Claims;
using BridgeportClaims.Data.DataProviders.Payments;
using BridgeportClaims.Data.DataProviders.UserOptions;
using BridgeportClaims.Excel.Adapters;
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
        private readonly IPaymentsDataProvider _paymentsDataProvider;


        public TestChecksController(IDbccUserOptionsProvider dbccUserOptionsProvider, 
            IClaimsDataProvider claimsDataProvider, IPaymentsDataProvider paymentsDataProvider)
        {
            _dbccUserOptionsProvider = dbccUserOptionsProvider;
            _claimsDataProvider = claimsDataProvider;
            _paymentsDataProvider = paymentsDataProvider;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("call")]
        public async Task<IHttpActionResult> InitializeTestCall()
        {
            try
            {
                var fileBytes = _paymentsDataProvider.GetBytesFromDbAsync("darkerv2.png");
                var dt = OleDbExcelAdapter.GetDataTableFromExcel(fileBytes, true);
                await _paymentsDataProvider.ImportDataTableIntoDbAsync(dt);
                var testClaimId = Convert.ToInt32(cs.GetAppSetting(c.TestClaimIdKey));
                var testUserId = cs.GetAppSetting(c.TestUserIdKey);
                _dbccUserOptionsProvider.IsSessionUsingReadCommittedSnapshotIsolation();
                var retVal = _claimsDataProvider.GetClaimsDataByClaimId(testClaimId, testUserId);
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
