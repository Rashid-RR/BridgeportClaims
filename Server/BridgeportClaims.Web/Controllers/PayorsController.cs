using NLog;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.Payors;
using BridgeportClaims.Entities.Automappers;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/payors")]
    public class PayorsController : ApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IPayorsDataProvider _payorsDataProvider;
        private readonly IPayorMapper _payorMapper;

        public PayorsController(IPayorsDataProvider payorsDataProvider, IPayorMapper payorMapper)
        {
            _payorsDataProvider = payorsDataProvider;
            _payorMapper = payorMapper;
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAllPayors()
        {
            try
            {
                return await Task.Run(() => 
                    Ok(_payorMapper.GetPayorViewModels(_payorsDataProvider.GetAllPayors().ToList())));
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
                    Ok(_payorsDataProvider.GetPaginatedPayors(pageNumber, pageSize).ToList()));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
