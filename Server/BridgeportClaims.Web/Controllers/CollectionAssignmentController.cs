using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.CollectionAssignments;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Web.Models;
using Microsoft.AspNet.Identity;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/collection")]
    public class CollectionAssignmentController : BaseApiController
    {
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);
        private readonly Lazy<ICollectionAssignmentProvider> _collectionAssignmentProvider;

        public CollectionAssignmentController(Lazy<ICollectionAssignmentProvider> collectionAssignmentProvider)
        {
            _collectionAssignmentProvider = collectionAssignmentProvider;
        }

        [HttpPost]
        [Route("get-collection-assignment-data")]
        public IHttpActionResult GetCollectionAssignmentData(string userId)
        {
            try
            {
                var results = _collectionAssignmentProvider.Value.GetCollectionAssignmentData(userId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        [HttpPost]
        [Route("assign-users-to-payors")]
        public async Task<IHttpActionResult> AssignUsersToPayors(CollectionAssignmentModel model)
        {
            try
            {
                if (null == model)
                    throw new ArgumentNullException(nameof(model));
                if (null == model.UserId)
                    throw new ArgumentNullException(nameof(model.UserId));
                if (null == model.PayorIds)
                    throw new ArgumentNullException(nameof(model.PayorIds));
                var user = await AppUserManager.FindByIdAsync(model.UserId).ConfigureAwait(false);
                if (default(int) == model.PayorIds.Count)
                {
                    Logger.Value.Info($"We are removing all associations (if there are any), from {user.FullName}");
                }
                IList<CarrierDto> carrierDtos =
                    model.PayorIds.Select(item => new CarrierDto { PayorID = item }).ToList();
                var dt = carrierDtos.ToFixedDataTable();
                var modifiedByUserId = User.Identity.GetUserId();
                _collectionAssignmentProvider.Value.MergeCollectionAssignments(model.UserId, modifiedByUserId, dt);
                return Ok(new { message = $"{user.FullName} was associated to the selected carriers successfully." });
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
