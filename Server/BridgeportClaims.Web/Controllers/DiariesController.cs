using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.Diaries;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/diary")]
    public class DiariesController : BaseApiController
    {
        private readonly IDiaryProvider _diaryProvider;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public DiariesController(IDiaryProvider diaryProvider)
        {
            _diaryProvider = diaryProvider;
        }

        [HttpPost]
        [Route("get")]
        public async Task<IHttpActionResult> GetDiaries(bool isDefaultSort, DateTime? startDate, DateTime? endDate,
            string sort, string sortDirection, int page, int pageSize)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var results = _diaryProvider.GetDiaries(isDefaultSort, startDate, endDate,
                        sort, sortDirection, page, pageSize);
                    return Ok(results);
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
