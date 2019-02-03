using System;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.DecisionTrees;
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

        public DecisionsTreeController(Lazy<IDecisionTreeDataProvider> decisionTreeDataProvider)
        {
            _decisionTreeDataProvider = decisionTreeDataProvider;
        }

        [HttpPost]
        [Route("select-tree")]
        public IHttpActionResult GetDecisionTree(int treeRootId, int claimId)
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

        [HttpPost]
        [Route("choose-tree-path")]
        public IHttpActionResult ChooseTreePath(Guid sessionId, int selectedTreeId)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                _decisionTreeDataProvider.Value.DecisionTreeUserPathInsert(sessionId, selectedTreeId, userId);
                return Ok(new { message = "Tree selection made successfully." });
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
