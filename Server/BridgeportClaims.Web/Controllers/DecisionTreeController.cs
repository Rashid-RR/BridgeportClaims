using System;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Common.Caching;
using BridgeportClaims.Common.Extensions;
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
        private readonly Lazy<IMemoryCacher> _memoryCacher;

        public DecisionsTreeController(Lazy<IDecisionTreeDataProvider> decisionTreeDataProvider)
        {
            _decisionTreeDataProvider = decisionTreeDataProvider;
            _memoryCacher = new Lazy<IMemoryCacher>(() => MemoryCacher.Instance);
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
        public IHttpActionResult ChooseTreePath(string sessionId, int parentTreeId, int selectedTreeId)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                if (sessionId.IsNullOrWhiteSpace())
                {
                    throw new Exception("Error, an invalid Session ID was created for this journey through the tree.");
                }
                if (!_memoryCacher.Value.Contains(sessionId))
                {
                    _memoryCacher.Value.AddOrGetExisting(sessionId, () => new Tuple<int, int>(parentTreeId, selectedTreeId));
                }
                _decisionTreeDataProvider.Value.DecisionTreeUserPathInsert(sessionId, parentTreeId, selectedTreeId, userId);
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
