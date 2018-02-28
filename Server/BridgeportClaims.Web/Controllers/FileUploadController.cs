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
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
		private readonly IImportFileProvider _importFileProvider;

		public FileUploadController(IImportFileProvider importFileProvider)
		{
			_importFileProvider = importFileProvider;
		}

		[HttpDelete]
		[Route("delete")]
		public async Task<IHttpActionResult> DeleteImportFile(int importFileId)
		{
			try
			{
			    return await Task.Run(() =>
			    {
			        _importFileProvider.DeleteImportFile(importFileId);
			        return Ok(new {message = "Deleted the Import File Successfully"});
			    }).ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
			}
		}

		[HttpGet]
		[Route("getfiles")]
		public async Task<IHttpActionResult> GetImportFiles()
		{
			try
			{
			    return await Task.Run(() =>
			    {
			        var files = _importFileProvider.GetImportFileDtos();
			        return Ok(files);
			    }).ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
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
							_importFileProvider.SaveFileToDatabase(file.Value, fileName, ext, description);
					}
					catch (Exception ex)
					{
						Logger.Error(ex,
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
				Logger.Error(ex);
				return Content(HttpStatusCode.NotAcceptable, ex.GetBaseException().Message);
			}
		}
	}
}