﻿using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BridgeportClaims.Common.FileUploading;
using BridgeportClaims.Web.Infrastructure;

namespace BridgeportClaims.Web.Controllers
{
    //[Authorize(Roles = "Admin")]
    [AllowAnonymous]
    [RoutePrefix("api/fileupload")]
    public class FileUploadController : ApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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
                int uploadType;
                //if (!int.TryParse(provider.FormData["uploadType"], out uploadType))
                //    return BadRequest("Upload Type is invalid.");

                // Check if files are on the request.
                if (!provider.FileStreams.Any())
                    return BadRequest("No file uploaded.");

                IList<string> uploadedFiles = new List<string>();
                foreach (var file in provider.FileStreams)
                {
                    var fileName = file.Key;

                    // Do something with the uploaded file
                    var fullFilePath = Path.Combine(Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory, "FileUploads"), fileName);

                    try
                    {
                        using (var fileToCopy = File.Create(fullFilePath))
                        {
                            StreamProvider.CopyStream(file.Value, fileToCopy);
                        }
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