using System;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.AdjustorSearches;
using BridgeportClaims.Web.Models;
using Microsoft.AspNet.Identity;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/adjustors")]
    public class AdjustorsController : BaseApiController
    {
        private readonly Lazy<IAdjustorDataProvider> _adjustorSearchProvider;
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);

        public AdjustorsController(Lazy<IAdjustorDataProvider> adjustorSearchProvider)
        {
            _adjustorSearchProvider = adjustorSearchProvider;
        }

        [HttpPost]
        [Route("adjustor-names")]
        public IHttpActionResult GetAdjustorNames(string adjustorName)
        {
            try
            {
                var results = _adjustorSearchProvider.Value.GetAdjustorNames(adjustorName ?? string.Empty);
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
        public IHttpActionResult GetAdjustorSearchResults(string searchText)
        {
            try
            {
                var results = _adjustorSearchProvider.Value.GetAdjustorSearchResults(searchText);
                return Ok(results);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("get-adjustors")]
        public IHttpActionResult GetAdjustor(AbstractSearchModel model)
        {
            try
            {
                var results = _adjustorSearchProvider.Value.GetAdjustors(
                    model.SearchText, model.Page, model.PageSize, model.Sort, model.SortDirection);
                return Ok(results);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("insert-adjustor")]
        public IHttpActionResult InsertAdjustor(AdjustorModel model)
        {
            try
            {
                var modifiedByUserId = User.Identity.GetUserId();
                var adjustor = _adjustorSearchProvider.Value.InsertAdjustor(model.AdjustorName, model.Address1,
                    model.Address2, model.City, model.StateId, model.PostalCode, model.PhoneNumber, model.FaxNumber, model.EmailAddress,
                    model.Extension, modifiedByUserId);
                return Ok(adjustor);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("update-adjustor")]
        public IHttpActionResult UpdateAdjustor(AdjustorModel model)
        {
            try
            {
                var modifiedByUserId = User.Identity.GetUserId();
                var adjustor = _adjustorSearchProvider.Value.UpdateAdjustor(model.AdjustorId, model.AdjustorName, model.Address1, model.Address2,
                    model.City, model.StateId, model.PostalCode, model.PhoneNumber, model.FaxNumber,
                    model.EmailAddress, model.Extension, modifiedByUserId);
                return Ok(adjustor);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("get-adjustor")]
        public IHttpActionResult GetAdjustor(int adjustorId)
        {
            try
            {
                var adjustor = _adjustorSearchProvider.Value.GetAdjustor(adjustorId);
                return Ok(adjustor);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
