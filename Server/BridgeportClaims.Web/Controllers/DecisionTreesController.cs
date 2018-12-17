using System;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.DecisionTrees;
using BridgeportClaims.Web.Framework.Models;
using Microsoft.AspNet.Identity;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/trees")]
    public class DecisionTreesController : BaseApiController
    {
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);
        private readonly Lazy<IDecisionTreeDataProvider> _decisionTreeDataProvider;

        public DecisionTreesController(Lazy<IDecisionTreeDataProvider> decisionTreeDataProvider)
        {
            _decisionTreeDataProvider = decisionTreeDataProvider;
        }

        [HttpPost]
        [Route("get-decision-tree")]
        public IHttpActionResult GetDecisionTree(int parentTreeId)
        {
            try
            {
                var results = _decisionTreeDataProvider.Value.GetDecisionTree(parentTreeId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("save-decision-tree")]
        public IHttpActionResult SaveDecisionTree(DecisionTreeModel model)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var decisionTree = _decisionTreeDataProvider.Value.InsertDecisionTree(model.ParentTreeId,
                    model.NodeName, model.NodeDescription, userId);
                return Ok(decisionTree);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
