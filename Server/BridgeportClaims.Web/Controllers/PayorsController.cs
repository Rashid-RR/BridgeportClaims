using NLog;
using System;
using System.Linq;
using System.Net;
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
        private readonly Lazy<IPayorsDataProvider> _payorsDataProvider;
        private readonly Lazy<IPayorSearchProvider> _payorSearchProvider;
        private readonly Lazy<IPayorMapper> _payorMapper;

        public PayorsController(
            Lazy<IPayorsDataProvider> payorsDataProvider, 
            Lazy<IPayorMapper> payorMapper, 
            Lazy<IPayorSearchProvider> payorSearchProvider)
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
                var results = _payorSearchProvider.Value.GetPayorSearchResults(searchText);
                return Ok(results);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpGet]
        public IHttpActionResult GetAllPayors()
        {
            try
            {
                return Ok(_payorMapper.Value.GetPayorViewModels(_payorsDataProvider.Value.GetAllPayors().ToList()));
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpGet]
        public IHttpActionResult GetPayors(int pageNumber, int pageSize)
        {
            try
            {
                return Ok(_payorsDataProvider.Value.GetPaginatedPayors(pageNumber, pageSize).ToList());
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
