using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.Diaries;
using BridgeportClaims.Web.Framework.Models;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/diary")]
    public class DiariesController : BaseApiController
    {
        private readonly Lazy<IDiaryProvider> _diaryProvider;
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);

        public DiariesController(Lazy<IDiaryProvider> diaryProvider)
        {
            _diaryProvider = diaryProvider;
        }

        [HttpPost]
        [Route("update-follow-up-date")]
        public IHttpActionResult UpdateFollowUpDate(FollowUpDateModel model)
        {
            try
            {
                if (null == model)
                    throw new ArgumentNullException(nameof(model));
                if (default (int) == model.DiaryId)
                    throw new ArgumentNullException(nameof(model.DiaryId));
                if (default (DateTime) == model.FollowUpDate.ToFormattedDateTime())
                    throw new ArgumentNullException(nameof(model.FollowUpDate));
                _diaryProvider.Value.UpdateDiaryFollowUpDate(model.DiaryId, model.FollowUpDate.ToFormattedDateTime());
                return Ok(new {message = "The follow-up date was updated successfully."});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("owners")]
        public IHttpActionResult GetDiaryOwners()
        {
            try
            {
                var owners = _diaryProvider.Value.GetDiaryOwners()?.OrderBy(x => x.Owner);
                return Ok(owners);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("remove")]
        public IHttpActionResult RemoveDiary(int prescriptionNoteId)
        {
            try
            {
                _diaryProvider.Value.RemoveDiary(prescriptionNoteId);
                return Ok(new {message = "The diary entry was removed successfully."});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("get")]
        public IHttpActionResult GetDiaries(DiariesViewModel model)
        {
            try
            {
                var results = _diaryProvider.Value.GetDiaries(model.IsDefaultSort, model.StartDate.ToNullableFormattedDateTime(), model.EndDate.ToNullableFormattedDateTime(),
                    model.Sort, model.SortDirection, model.Page, model.PageSize, model.Closed, model.UserId, model.NoteText);
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
