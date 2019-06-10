using System;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.AttorneyProviders;
using BridgeportClaims.Data.DataProviders.Clients;
using BridgeportClaims.Web.Models;
using Microsoft.AspNet.Identity;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/attorney")]
    public class AttorneyController : BaseApiController
    {
        private readonly Lazy<IAttorneyProvider> _attorneyProvider;
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);
        private readonly Lazy<IClientDataProvider> _clientDataProvider;

        public AttorneyController(Lazy<IAttorneyProvider> attorneyProvider,
            Lazy<IClientDataProvider> clientDataProvider)
        {
            _attorneyProvider = attorneyProvider;
            _clientDataProvider = clientDataProvider;
        }

        [HttpPost]
        [Route("attorney-names")]
        public IHttpActionResult GetAttorneyNames(string attorneyName)
        {
            try
            {
                var results = _attorneyProvider.Value.GetAttorneyNames(attorneyName ?? string.Empty);
                return Ok(results);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("get-states")]
        public IHttpActionResult GetStates()
        {
            try
            {
                var states = _clientDataProvider.Value.GetUsStates();
                return Ok(states);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("get-attorneys")]
        public IHttpActionResult GetAttorneys(AbstractSearchModel model)
        {
            try
            {
                var results = _attorneyProvider.Value.GetAttorneys(model.SearchText, model.Page, model.PageSize,
                    model.Sort
                    , model.SortDirection);
                return Ok(results);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("get-attorney")]
        public IHttpActionResult GetAttorney(int attorneyId)
        {
            try
            {
                var attorney = _attorneyProvider.Value.GetAttorney(attorneyId);
                return Ok(attorney);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("insert-attorney")]
        public IHttpActionResult InsertAttorney(AttorneyModel model)
        {
            try
            {
                var stateId = model.StateId.IsNullOrWhiteSpace() ? null :
                    int.TryParse(model.StateId, out var ti) ? ti : (int?)null;
                if (stateId == 0 || stateId == -1)
                {
                    stateId = null;
                }
                var modifiedByUserId = User.Identity.GetUserId();
                var attorney = _attorneyProvider.Value.InsertAttorney(model.AttorneyName, model.Address1,
                    model.Address2, model.City, stateId, model.PostalCode, model.PhoneNumber, model.FaxNumber, model.EmailAddress, modifiedByUserId);
                return Ok(attorney);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("update-attorney")]
        public IHttpActionResult UpdateAttorney(AttorneyModel model)
        {
            try
            {
                var stateId = model.StateId.IsNullOrWhiteSpace() ? null :
                    int.TryParse(model.StateId, out var ti) ? ti : (int?) null;
                if (stateId == 0 || stateId == -1)
                {
                    stateId = null;
                }
                var modifiedByUserId = User.Identity.GetUserId();
                var attorney = _attorneyProvider.Value.UpdateAttorney(model.AttorneyId, model.AttorneyName, model.Address1,
                    model.Address2, model.City, stateId, model.PostalCode, model.PhoneNumber, model.FaxNumber, model.EmailAddress, modifiedByUserId);
                return Ok(attorney);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
