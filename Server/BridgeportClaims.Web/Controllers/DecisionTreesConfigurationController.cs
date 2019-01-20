using System;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.DecisionTrees;
using BridgeportClaims.Data.Trees;
using BridgeportClaims.Web.Models;
using Microsoft.AspNet.Identity;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/tree-config")]
    public class DecisionTreesConfigurationController : BaseApiController
    {
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);
        private readonly Lazy<IDecisionTreeDataProvider> _decisionTreeDataProvider;

        public DecisionTreesConfigurationController(Lazy<IDecisionTreeDataProvider> decisionTreeDataProvider)
        {
            _decisionTreeDataProvider = decisionTreeDataProvider;
        }

        [HttpPost]
        [Route("get-tree")]
        public IHttpActionResult GetDecisionTree(int parentTreeId)
        {
            try
            {
                var tree = _decisionTreeDataProvider.Value.GetDecisionTree(parentTreeId);
                var hierarchy = tree.ToHierarchy(parentTreeId);
                return Ok(hierarchy);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("insert-node")]
        public IHttpActionResult InsertNode(DecisionTreeModel model)
        {
            try
            {
                if (null == model)
                {
                    throw new ArgumentNullException(nameof(model));
                }
                var userId = User.Identity.GetUserId();
                // Tuple deconstruction.
                var (tree, rootTreeId) = _decisionTreeDataProvider.Value.InsertDecisionTree(model.ParentTreeId,
                    model.NodeName, userId);
                var hierarchy = tree.ToHierarchy(rootTreeId);
                return Ok(hierarchy);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("delete-node")]
        public IHttpActionResult DeleteNode(int treeId)
        {
            try
            {
                var (tree, rootTreeId) = _decisionTreeDataProvider.Value.DeleteDecisionTree(treeId);
                var hierarchy = tree.ToHierarchy(rootTreeId);
                return Ok(hierarchy);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("get-decision-tree-list")]
        public IHttpActionResult SaveDecisionTree(AbstractSearchModel model)
        {
            try
            {
                var list = _decisionTreeDataProvider.Value.GetDecisionTreeList(model.SearchText, model.Sort, model.SortDirection,
                    model.Page, model.PageSize);
                return Ok(list);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
