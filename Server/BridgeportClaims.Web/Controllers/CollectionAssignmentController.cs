using System;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.CollectionAssignments;
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
    }
}
