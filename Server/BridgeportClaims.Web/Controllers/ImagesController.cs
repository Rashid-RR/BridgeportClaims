using System;
using System.Net;
using System.Web.Http;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.ClaimImages;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Entities.DomainModels;
using BridgeportClaims.Web.Models;
using NLog;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/image")]
    public class ImagesController : BaseApiController
    {
        private static readonly Lazy<Logger> Logger = new Lazy<Logger>(LogManager.GetCurrentClassLogger);
        private readonly IClaimImageProvider _claimImageProvider;
        private readonly IRepository<DocumentIndex> _documentIndexRepository;
        private readonly IRepository<DocumentType> _documentTypeRepository;

        public ImagesController(
            IClaimImageProvider claimImageProvider, 
            IRepository<DocumentIndex> documentIndexRepository, 
            IRepository<DocumentType> documentTypeRepository)
        {
            _claimImageProvider = claimImageProvider;
            _documentIndexRepository = documentIndexRepository;
            _documentTypeRepository = documentTypeRepository;
        }

        [HttpPost]
        [Route("get")]
        public IHttpActionResult GetClaimImages(ClaimImageViewModel model)
        {
            try
            {
                if (null == model)
                    throw new ArgumentNullException(nameof(model));
                var results = _claimImageProvider.GetClaimImages(model.ClaimId, model.Sort, model.SortDirection, model.Page, model.PageSize);
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
                var docIndex = _documentIndexRepository.Get(model.DocumentId);
                if (null == docIndex)
                    throw new Exception($"Error, the image could not be found with the Id of {model.DocumentId}.");
                docIndex.RxDate = model.RxDate.ToNullableFormattedDateTime();
                docIndex.RxNumber = model.RxNumber;
                var docType = _documentTypeRepository.Get(model.DocumentTypeId);
                docIndex.DocumentType = docType;
                docIndex.UpdatedOnUtc = DateTime.UtcNow;
                _documentIndexRepository.Update(docIndex);
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
                var entity = _documentIndexRepository.Get(documentId);
                if (null == entity)
                    throw new Exception($"Error, could not find the image with Id: {documentId}.");
                _documentIndexRepository.Delete(entity);
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
