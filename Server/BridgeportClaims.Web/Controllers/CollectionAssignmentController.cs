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
    }
}
