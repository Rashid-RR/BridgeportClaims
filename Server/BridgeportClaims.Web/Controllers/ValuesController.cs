using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Business.Logging;
using BridgeportClaims.Data.DataProviders;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Entities.DomainModels;

namespace BridgeportClaims.Web.Controllers
{
    //[Authorize]
    public class ValuesController : ApiController
    {
        private readonly ILoggingService _loggingService;
        private readonly IDbccUserOptionsProvider _dbccUserOptionsProvider;
        private readonly IRepository<Claim> _claimRepository;
        private readonly IRepository<Payment> _paymentRepository;

        public ValuesController(ILoggingService loggingService, IDbccUserOptionsProvider dbccUserOptionsProvider, IRepository<Claim> claimRepository, IRepository<Payment> paymentRepository)
        {
            _loggingService = loggingService;
            _dbccUserOptionsProvider = dbccUserOptionsProvider;
            _claimRepository = claimRepository;
            _paymentRepository = paymentRepository;
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
                            //IsSessionUsingReadCommittedSnapshotIsolation =
                            //_dbccUserOptionsProvider.IsSessionUsingReadCommittedSnapshotIsolation(),
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

        [HttpGet]
        public IHttpActionResult TestMeAgain()
        {
            var claim = (from c in _claimRepository.GetAll()
                join r in _paymentRepository.GetAll() on c.Id equals r.Claim.Id
                select c).ToList();
            return Ok(claim);
        }
    }
}
