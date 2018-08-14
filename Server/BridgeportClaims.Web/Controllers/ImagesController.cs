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
        private readonly Lazy<IClaimImageProvider> _claimImageProvider;
        private readonly Lazy<IRepository<DocumentIndex>> _documentIndexRepository;
        private readonly Lazy<IRepository<DocumentType>> _documentTypeRepository;

        public ImagesController(
            Lazy<IClaimImageProvider> claimImageProvider, 
            Lazy<IRepository<DocumentIndex>> documentIndexRepository, 
            Lazy<IRepository<DocumentType>> documentTypeRepository)
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
                _claimImageProvider.Value.UpdateDocumentIndex(model.DocumentId,
                    model.RxDate.ToNullableFormattedDateTime(), model.RxNumber, model.DocumentTypeId);
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
                var entity = _documentIndexRepository.Value.Get(documentId);
                if (null == entity)
                    throw new Exception($"Error, could not find the image with Id: {documentId}.");
                _documentIndexRepository.Value.Delete(entity);
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
