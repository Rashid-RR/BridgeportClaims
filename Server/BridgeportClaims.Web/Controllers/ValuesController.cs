using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    //[Authorize]
    public class ValuesController : ApiController
    {
        private readonly IDbccUserOptionsProvider _dbccUserOptionsProvider;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ValuesController(IDbccUserOptionsProvider dbccUserOptionsProvider)
        {
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
                Logger.Error(ex);
                throw;
            }
        }

        public IEnumerable<string> Get() => new[] {"ValueOneNonAsync", "ValueTwoNonAsync"};

        // GET api/values/5
        public string Get(int id)
        {
            var retVal = $"value with id {id} coming in...";
            Logger.Info(retVal);
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
                Logger.Error(ex);
                throw;
            }
        }

    
    }
}
