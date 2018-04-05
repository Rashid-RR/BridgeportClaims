using NLog;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.Payors;
using BridgeportClaims.Data.DataProviders.PayorSearches;
using BridgeportClaims.Entities.Automappers;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/payors")]
    public class PayorsController : BaseApiController
    {
        private static readonly Lazy<Logger> Logger = new Lazy<Logger>(LogManager.GetCurrentClassLogger);
        private readonly IPayorsDataProvider _payorsDataProvider;
        private readonly IPayorSearchProvider _payorSearchProvider;
        private readonly IPayorMapper _payorMapper;

        public PayorsController(IPayorsDataProvider payorsDataProvider, IPayorMapper payorMapper, IPayorSearchProvider payorSearchProvider)
        {
            _payorsDataProvider = payorsDataProvider;
            _payorMapper = payorMapper;
            _payorSearchProvider = payorSearchProvider;
        }

        [HttpPost]
        [Route("search")]
        public IHttpActionResult GetPayorSearchResults(string searchText)
        {
            try
            {
                var results = _payorSearchProvider.GetPayorSearchResults(searchText);
                return Ok(results);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
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
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
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
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
