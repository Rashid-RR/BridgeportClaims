using System;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.ClaimImages;
using BridgeportClaims.Web.Framework.Models;
using Microsoft.AspNet.Identity;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/image")]
    public class ImagesController : BaseApiController
    {
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);
        private readonly Lazy<IClaimImageProvider> _claimImageProvider;

        public ImagesController(Lazy<IClaimImageProvider> claimImageProvider)
        {
            _claimImageProvider = claimImageProvider;
        }

        [HttpPost]
        [Route("get")]
        public IHttpActionResult GetClaimImages(ClaimSortViewModel model)
        {
            try
            {
                if (null == model)
                    throw new ArgumentNullException(nameof(model));
                var results = _claimImageProvider.Value.GetClaimImages(model.ClaimId, model.Sort, model.SortDirection, model.Page, model.PageSize);
                return Ok(results);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpPut]
        [Route("edit")]
        public IHttpActionResult EditClaimImage(EditClaimImageViewModel model)
        {
            try
            {
                if (null == model)
                    throw new ArgumentNullException(nameof(model));
                var docId = model.DocumentId;
                var claimId = model.ClaimId;
                var documentTypeId = model.DocumentTypeId;
                var theRxDate = model.RxDate?.ToNullableFormattedDateTime();
                var rxNumber = model.RxNumber;
                var invoiceNumber = model.InvoiceNumber;
                var injuryDate = model.InjuryDate;
                var attorneyName = model.AttorneyDate;
                var indexedByUserId = User.Identity.GetUserId();
                var rxDate = model.RxDate;
                _claimImageProvider.Value.UpdateDocumentIndex(docId, claimId, documentTypeId, theRxDate, rxNumber,
                    invoiceNumber, injuryDate, attorneyName, indexedByUserId);
                return Ok(new {message = "The image was updated successfully."});
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        [HttpDelete]
        [Route("reindex")]
        public IHttpActionResult ReindexClaimImage(int documentId)
        {
            try
            {
                _claimImageProvider.Value.ReindexDocumentImage(documentId);
                return Ok(new { message = "The image was reindexed successfully." });
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }
    }
}
