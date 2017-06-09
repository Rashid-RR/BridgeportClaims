using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Business.Logging;
using BridgeportClaims.Data.DataProviders;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Data.RepositoryUnitOfWork;
using BridgeportClaims.Data.SessionFactory;
using BridgeportClaims.Entities.DomainModels;

namespace BridgeportClaims.Web.Controllers
{
    //[Authorize]
    public class ValuesController : ApiController
    {
        private readonly ILoggingService _loggingService;
        private readonly IDbccUserOptionsProvider _dbccUserOptionsProvider;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Claim> _claimRepository;
        private readonly IRepository<Payment> _paymentRepository;

        public ValuesController(ILoggingService loggingService, IDbccUserOptionsProvider dbccUserOptionsProvider, IUnitOfWork unitOfWork, IRepository<Claim> claimRepository, IRepository<Payment> paymentRepository)
        {
            _loggingService = loggingService;
            _dbccUserOptionsProvider = dbccUserOptionsProvider;
            _unitOfWork = unitOfWork;
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

        [HttpGet]
        public async Task<IHttpActionResult> TestMeAgain()
        {
            return await Task.Run(() =>
            {
                using (var uow = new UnitOfWork(SessionFactoryBuilder.CreateSessionFactory()))
                {
                    uow.BeginTransaction();
                    var claim = _claimRepository.Load(14);
                    claim.DateOfInjury = DateTime.Now;
                    var payment = _paymentRepository.GetMany(x => x.Id == 691).First();
                    payment.AmountPaid = -2;
                    _unitOfWork.Commit();
                    return Ok(claim);
                }
            });
        }
    }
}
