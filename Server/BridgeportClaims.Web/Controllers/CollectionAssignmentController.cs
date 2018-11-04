using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.CollectionAssignments;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
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
        [Route("delete")]
        public IHttpActionResult DeleteCollectionAssignment(string userId, int payorId)
        {
            try
            {
                _collectionAssignmentProvider.Value.DeleteCollectionAssignment(userId, payorId);
                return Ok(new {message = "The payor was removed from the user successfully."});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
