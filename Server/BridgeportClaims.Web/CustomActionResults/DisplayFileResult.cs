using BridgeportClaims.Common.Extensions;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace BridgeportClaims.Web.CustomActionResults
{
    public class DisplayFileResult : IHttpActionResult
    {
        private readonly string _filePath;
        private readonly string _fileName;
        private readonly string _contentType;

        public DisplayFileResult(string filePath, string fileName, string contentType = null)
        {
            _filePath = filePath.IsNullOrWhiteSpace() ? throw new ArgumentNullException(nameof(filePath)) : filePath;
            _fileName = fileName.IsNullOrWhiteSpace() ? throw new ArgumentNullException(nameof(fileName)) : fileName;
            _contentType = contentType;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(File.OpenRead(_filePath))
            };
            var contentType = _contentType ?? MimeMapping.GetMimeMapping(Path.GetExtension(_filePath));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            if (ContentDispositionHeaderValue.TryParse("inline; filename=" + _fileName, out var contentDisposition))
            {
                response.Content.Headers.ContentDisposition = contentDisposition;
            }
            return Task.FromResult(response);
        }
    }
}