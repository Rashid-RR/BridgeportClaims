using NLog;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.Claims;
using BridgeportClaims.Data.DataProviders.UserOptions;
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

        public TestChecksController(IDbccUserOptionsProvider dbccUserOptionsProvider, 
            IClaimsDataProvider claimsDataProvider)
        {
            _dbccUserOptionsProvider = dbccUserOptionsProvider;
            _claimsDataProvider = claimsDataProvider;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("call")]
        public async Task<IHttpActionResult> InitializeTestCall()
        {
            try
            {
                return await Task.Run(() =>
                {
                    var testClaimId = Convert.ToInt32(cs.GetAppSetting(c.TestClaimIdKey));
                    var testUserId = cs.GetAppSetting(c.TestUserIdKey);
                    _dbccUserOptionsProvider.IsSessionUsingReadCommittedSnapshotIsolation();
                    var retVal = _claimsDataProvider.GetClaimsDataByClaimId(testClaimId, testUserId);
                    return Ok(retVal);
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
