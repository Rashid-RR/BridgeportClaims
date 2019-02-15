using System;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Common.Caching;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.DecisionTrees;
using BridgeportClaims.Web.Models;
using Microsoft.AspNet.Identity;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/trees")]
    public class DecisionsTreeController : BaseApiController
    {
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);
        private readonly Lazy<IDecisionTreeDataProvider> _decisionTreeDataProvider;
        private readonly Lazy<IMemoryCacher> _memoryCacher;

        public DecisionsTreeController(Lazy<IDecisionTreeDataProvider> decisionTreeDataProvider)
        {
            _decisionTreeDataProvider = decisionTreeDataProvider;
            _memoryCacher = new Lazy<IMemoryCacher>(() => MemoryCacher.Instance);
        }

        [HttpPost]
        [Route("select-tree")]
        public IHttpActionResult GetDecisionTree(TreeExperienceModel model)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var sessionId = _decisionTreeDataProvider.Value.DecisionTreeHeaderInsert(userId, treeRootId, claimId);
                return Ok(new {message = "Tree selected successfully.", sessionId});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
