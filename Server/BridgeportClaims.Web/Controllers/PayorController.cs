using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Business.Logging;
using BridgeportClaims.Data.Services.Payors;

namespace BridgeportClaims.Web.Controllers
{
    public class PayorController : ApiController
    {
        private readonly ILoggingService _loggingService;
        private readonly IPayorService _payorService;

        public PayorController(ILoggingService loggingService, IPayorService payorService)
        {
            _loggingService = loggingService;
            _payorService = payorService;
        }
        
        [HttpGet]
        public async Task<IHttpActionResult> GetAllPayors()
        {
            try
            {
                return await Task.Run(() => Ok(_payorService.GetAllPayors()));
            }
            catch (Exception ex)
            {
                _loggingService.Error(ex, this.GetType().Name, MethodBase.GetCurrentMethod()?.Name);
                throw;
            }
        }
    }
}
