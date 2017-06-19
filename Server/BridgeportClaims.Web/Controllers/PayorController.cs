using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.Payors;
using BridgeportClaims.Entities.Automappers;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize]
    public class PayorController : BaseApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IPayorService _payorService;
        private readonly IPayorMapper _payorMapper;

        public PayorController(IPayorService payorService, IPayorMapper payorMapper)
        {
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
                Logger.Error(ex);
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
                Logger.Error(ex);
                throw;
            }
        }
    }
}
