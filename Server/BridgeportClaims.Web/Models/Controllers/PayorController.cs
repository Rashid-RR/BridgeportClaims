using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Business.Logging;
using BridgeportClaims.Data.Services.Payors;
using BridgeportClaims.Entities.Automappers;

namespace BridgeportClaims.Web.Controllers
{
    public class PayorController : ApiController
    {
        private readonly ILoggingService _loggingService;
        private readonly IPayorService _payorService;
        private readonly IPayorMapper _payorMapper;

        public PayorController(
            ILoggingService loggingService, 
            IPayorService payorService, 
            IPayorMapper payorMapper)
        {
            _loggingService = loggingService;
            _payorService = payorService;
            _payorMapper = payorMapper;
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAllPayors()
        {
            try
            {
                return await Task.Run(() => 
                    Ok(_payorMapper.GetPayorViewModels(_payorService.GetAllPayors().ToList())));
            }
            catch (Exception ex)
            {
                _loggingService.Error(ex, this.GetType().Name, MethodBase.GetCurrentMethod()?.Name);
                throw;
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetPayors(int pageNumber, int pageSize)
        {
            try
            {
                return await Task.Run(() =>
                    Ok(_payorService.GetPaginatedPayors(pageNumber, pageSize).ToList()));
            }
            catch (Exception ex)
            {
                _loggingService.Error(ex, this.GetType().Name, MethodBase.GetCurrentMethod()?.Name);
                throw;
            }
        }
    }
}
