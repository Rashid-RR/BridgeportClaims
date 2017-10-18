﻿using System;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.Diaries;
using BridgeportClaims.Web.Models;
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
        [Route("remove")]
        public IHttpActionResult RemoveDiary(int prescriptionNoteId)
        {
            try
            {
                _diaryProvider.RemoveDiary(prescriptionNoteId);
                return Ok(new {message = "The diary entry was removed successfully."});
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("get")]
        public IHttpActionResult GetDiaries(DiariesViewModel model)
        {
            try
            {
                var results = _diaryProvider.GetDiaries(model.IsDefaultSort, model.StartDate, model.EndDate,
                    model.Sort, model.SortDirection, model.Page, model.PageSize, model.Closed);
                return Ok(results);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
            }
        }
    }
}
