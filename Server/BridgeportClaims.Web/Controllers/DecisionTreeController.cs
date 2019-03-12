using NLog;
using System;
using System.Net;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using BridgeportClaims.Web.Models;
using BridgeportClaims.Data.DataProviders.DecisionTrees;

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
        [Route("save-tree-experience")]
        public IHttpActionResult GetDecisionTree(TreeExperienceModel model)
        {
            try
            {
                var modifiedByUserId = User.Identity.GetUserId();
                _decisionTreeDataProvider.Value.SaveDecisionTreeChoice(model.RootTreeId, model.LeafTreeId,
                    model.ClaimId, model.EpisodeTypeId, model.PharmacyNabp,
                    model.RxNumber, model.EpisodeText, modifiedByUserId);
                return Ok(new {message = "The episode and tree path were saved successfully."});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
