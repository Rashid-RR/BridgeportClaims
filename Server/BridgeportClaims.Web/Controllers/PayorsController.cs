using NLog;
using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.Payors;
using BridgeportClaims.Data.DataProviders.PayorSearches;
using BridgeportClaims.Web.Models;
using Microsoft.AspNet.Identity;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/payors")]
    public class PayorsController : BaseApiController
    {
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);
        private readonly Lazy<IPayorsDataProvider> _payorsDataProvider;
        private readonly Lazy<IPayorSearchProvider> _payorSearchProvider;

        public PayorsController(
            Lazy<IPayorsDataProvider> payorsDataProvider,
            Lazy<IPayorSearchProvider> payorSearchProvider)
        {
            _payorsDataProvider = payorsDataProvider;
            _payorSearchProvider = payorSearchProvider;
        }

        [HttpPost]
        [Route("get-payors-for-unpaid-scripts")]
        public IHttpActionResult GetPayorsForUnpaidScripts()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var results = _payorsDataProvider.Value.GetPayors(userId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("get-payors")]
        public IHttpActionResult GetPayors()
        {
            try
            {
                var results = _payorsDataProvider.Value.GetPayors();
                return Ok(results);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
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
        [Route("get-payor-by-id")]
        public IHttpActionResult GetPayorById(int payorId)
        {
            try
            {
                return Ok(_payorsDataProvider.Value.GetPayor(payorId));
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
                return Ok(_payorsDataProvider.Value.GetAllPayors());
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
                return Ok(_payorsDataProvider.Value.GetAllPayors());
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("get-references-payors-list")]
        public IHttpActionResult GetReferencesPayorsList(AbstractSearchModel model)
        {
            try
            {
                return Ok(_payorsDataProvider.Value.GetPayorList(model.SearchText, model.Page, model.PageSize,
                    model.Sort, model.SortDirection));
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("references-payor-insert")]
        public IHttpActionResult PayorInsert(PayorEntityModel model)
        {
            try
            {
                var modifiedByUserId = User.Identity.GetUserId();
                var payor = _payorsDataProvider.Value.PayorInsert(model.GroupName, model.BillToName, model.BillToAddress1,
                    model.BillToAddress2, model.BillToCity, model.BillToStateId, model.BillToPostalCode, model.PhoneNumber,
                    model.AlternatePhoneNumber, model.FaxNumber, model.Notes, model.Contact, model.LetterName, modifiedByUserId);
                return Ok(payor);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("references-payor-update")]
        public IHttpActionResult PayorUpdate(PayorEntityModel model)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var payor = _payorsDataProvider.Value.PayorUpdate(model.PayorId, model.GroupName, model.BillToName, model.BillToAddress1,
                    model.BillToAddress2, model.BillToCity, model.BillToStateId, model.BillToPostalCode, model.PhoneNumber,
                    model.AlternatePhoneNumber, model.FaxNumber, model.Notes, model.Contact, model.LetterName, userId);
                return Ok(payor);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
