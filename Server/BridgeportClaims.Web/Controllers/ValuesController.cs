using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Business.Logging;
using BridgeportClaims.Data.DataProviders;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize]
    public class ValuesController : ApiController
    {
        private readonly ILoggingService _loggingService;
        private readonly IDbccUserOptionsProvider _dbccUserOptionsProvider;

        public ValuesController(ILoggingService loggingService, IDbccUserOptionsProvider dbccUserOptionsProvider)
        {
            _loggingService = loggingService;
            _dbccUserOptionsProvider = dbccUserOptionsProvider;
        }

        // GET api/values
        public async Task<IEnumerable<string>> GetValues()
        {
            try
            {
                return await Task.FromResult(new[] {"ValueOneTwoThree", "ValueOneTwoThreeFour"});
            }
            catch (Exception ex)
            {
                _loggingService.Error(ex, GetType().Name, MethodBase.GetCurrentMethod()?.Name);
                throw;
            }
        }

        public IEnumerable<string> Get() => new[] {"ValueOneNonAsync", "ValueTwoNonAsync"};

        // GET api/values/5
        public string Get(int id)
        {
            var retVal = $"value with id {id} coming in...";
            _loggingService.Error(retVal, GetType().Name, MethodBase.GetCurrentMethod()?.Name);
            return retVal;
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        [HttpGet]
        //[CamelCasedApiMethod]
        public async Task<IHttpActionResult> TestMe()
        {
            try
            {
                return
                    await Task.Run(() =>
                    {
                        var data = new
                        {
                            IsSessionUsingReadCommittedSnapshotIsolation =
                            _dbccUserOptionsProvider.IsSessionUsingReadCommittedSnapshotIsolation(),
                            UrlToPost = "HttpPost"
                        };
                        return Ok(data);
                    });
            }
            catch (Exception ex)
            {
                _loggingService.Error(ex, GetType().Name, MethodBase.GetCurrentMethod()?.Name);
                throw;
            }
        }
    }
}
