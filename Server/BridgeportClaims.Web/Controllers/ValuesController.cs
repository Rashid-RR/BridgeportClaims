using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Business.Logging;
using BridgeportClaims.Web.Attributes;
using BridgeportClaims.Web.Models;

namespace BridgeportClaims.Web.Controllers
{
    //[Authorize]
    public class ValuesController : ApiController
    {
        private readonly ILoggingService _loggingService;

        public ValuesController(ILoggingService loggingService)
        {
            this._loggingService = loggingService;
        }

        // GET api/values
        public async Task<IEnumerable<string>> GetValues() => await Task.FromResult(new string[] {"ValueOneTwoThree", "ValueOneTwoThreeFour" });

        public IEnumerable<string> Get() => new[] {"ValueOneNonAsync", "ValueTwoNonAsync"};

        // GET api/values/5
        public string Get(int id)
        {
            var retVal = $"value with id {id} coming in...";
            this._loggingService.Error(retVal, this.GetType().Name, MethodBase.GetCurrentMethod()?.Name);
            return retVal;
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        [HttpGet]
        //[CamelCasedApiMethod]
        public IHttpActionResult TestMe()
        {
            var data = new
            {
                NameOfMe = "Jordan Gurney",
                UrlToPost = "HttpPost",
                FooFum = "California"
            };
            return Ok(data);
        }
    }
}
