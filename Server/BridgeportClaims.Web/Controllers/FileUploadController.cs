using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Data.DataProviders.ImportFile;
using BridgeportClaims.Web.Infrastructure;

namespace BridgeportClaims.Web.Controllers
{
	[RoutePrefix("api/fileupload")]
	[Authorize(Roles = "Admin")]
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
					ImportFileProvider.DeleteImportFile(importFileId);
					return Ok(new { message = "Deleted the Import File Successfully"});
				});
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				throw;
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
				});
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				throw;
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
				await Request.Content.ReadAsMultipartAsync(provider);

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
							ImportFileProvider.SaveFileToDatabase(file.Value, fileName, ext, description);
					}
					catch (Exception ex)
					{
						Logger.Error(ex,
							$"The file: {fileName}, with the description: {description} was not copied successfully");
						throw;
					}
					finally
					{
						file.Value.Dispose();
					}


					// Example.
					// UploadManager.Upload(stream, fileName, uploadType, description);

					// Keep track of the filename for the response
					uploadedFiles.Add(fileName);
				}

				return Ok(new {message = "Successfully Uploaded: " + string.Join(", ", uploadedFiles)});
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				return Content(HttpStatusCode.InternalServerError, ex.GetBaseException().Message);
			}
		}
	}
}