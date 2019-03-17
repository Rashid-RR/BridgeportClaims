using NLog;
using System;
using System.Net;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using BridgeportClaims.Web.Models;
using BridgeportClaims.Data.DataProviders.DecisionTrees;
using BridgeportClaims.Data.Dtos;

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
        public IHttpActionResult SaveTreeExperience(TreeExperienceModel model)
        {
            try
            {
                var modifiedByUserId = User.Identity.GetUserId();
                var episodeBlade = _decisionTreeDataProvider.Value.SaveDecisionTreeChoice(model.LeafTreeId,
                    model.ClaimId, model.EpisodeTypeId, model.PharmacyNabp,
                    model.RxNumber, model.EpisodeText, modifiedByUserId);
                var retVal = new NewEpisodeSaveDto
                {
                    Episode = episodeBlade, Message = "The episode and tree path were saved successfully."
                };
                return Ok(retVal);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("get-upline")]
        public IHttpActionResult GetUpline(int leafTreeId)
        {
            try
            {
                return Ok(_decisionTreeDataProvider.Value.GetUpline(leafTreeId));
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
