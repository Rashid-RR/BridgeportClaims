using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.ImportFiles;
using BridgeportClaims.Web.Infrastructure;

namespace BridgeportClaims.Web.Controllers
{
	[Authorize(Roles = "Admin")]
	[RoutePrefix("api/fileupload")]
	public class FileUploadController : BaseApiController
	{
		private static readonly Lazy<Logger> Logger = new Lazy<Logger>(LogManager.GetCurrentClassLogger);
		private readonly Lazy<IImportFileProvider> _importFileProvider;

		public FileUploadController(Lazy<IImportFileProvider> importFileProvider)
		{
			_importFileProvider = importFileProvider;
		}

		[HttpDelete]
		[Route("delete")]
		public IHttpActionResult DeleteImportFile(int importFileId)
		{
		    try
		    {
		        _importFileProvider.Value.DeleteImportFile(importFileId);
		        return Ok(new {message = "Deleted the Import File Successfully"});
		    }
		    catch (Exception ex)
		    {
		        Logger.Value.Error(ex);
		        return Content(HttpStatusCode.NotAcceptable, new {message = ex.Message});
		    }
		}

		[HttpGet]
		[Route("getfiles")]
		public IHttpActionResult GetImportFiles()
		{
			try
			{
			        var files = _importFileProvider.Value.GetImportFileDtos();
			        return Ok(files);
			}
			catch (Exception ex)
			{
				Logger.Value.Error(ex);
				return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
			}
		}

		[HttpPost]
		[Route("upload")]
		public async Task<IHttpActionResult> Upload()
		{
			try
			{
				if (!Request.Content.IsMimeMultipartContent())
					return Content(HttpStatusCode.UnsupportedMediaType, "Unsupported media type.");

				// Read the file and form data.
				var provider = new MultipartFormDataMemoryStreamProvider();
			    await Request.Content.ReadAsMultipartAsync(provider).ConfigureAwait(false);

				// Extract the fields from the form data.
				var description = provider.FormData["description"];

				// Check if files are on the request.
				if (!provider.FileStreams.Any())
					return BadRequest("No file uploaded.");

				IList<string> uploadedFiles = new List<string>();
				foreach (var file in provider.FileStreams)
				{
					var fileName = file.Key;
					var ext = Path.GetExtension(fileName);
					try
					{
						if (0 < file.Value.Length)
							_importFileProvider.Value.SaveFileToDatabase(file.Value, fileName, ext, description);
					}
					catch (Exception ex)
					{
						Logger.Value.Error(ex,
							$"The file: {fileName}, with the description: {description} was not copied successfully");
						return Content(HttpStatusCode.NotAcceptable, ex.GetBaseException().Message);
					}
					finally
					{
						file.Value.Dispose();
					}
					uploadedFiles.Add(fileName);
				}
				return Ok(new {message = "Successfully Uploaded: " + string.Join(", ", uploadedFiles)});
			}
			catch (Exception ex)
			{
				Logger.Value.Error(ex);
				return Content(HttpStatusCode.NotAcceptable, ex?.GetBaseException()?.Message);
			}
		}
	}
}