using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.DecisionTrees;
using BridgeportClaims.Data.Trees;
using BridgeportClaims.Web.Models;
using Microsoft.AspNet.Identity;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
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
        [Authorize(Roles = "Admin")]
        public IHttpActionResult InsertNode(DecisionTreeModel model)
        {
            try
            {
                if (null == model)
                {
                    throw new ArgumentNullException(nameof(model));
                }
                var userId = User.Identity.GetUserId();
                var node = _decisionTreeDataProvider.Value.InsertDecisionTree(model.ParentTreeId,
                    model.NodeName, userId);
                var tree = new Tree
                {
                    TreeId = node.TreeId,
                    NodeName = node.NodeName,
                    NodeDescription = node.NodeDescription,
                    TreeLevel = node.TreeLevel,
                    Children = new List<Node>()
                };
                return Ok(tree);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }

        [HttpPost]
        [Route("delete-node")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult DeleteNode(int treeId)
        {
            try
            {
                var childrenDeleted = _decisionTreeDataProvider.Value.DeleteDecisionTree(treeId);
                return Ok(new
                {
                    message =
                        $"The tree node {(childrenDeleted > 1 ? $"and {childrenDeleted} of its children nodes were" : "was")} deleted successfully."
                });
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
