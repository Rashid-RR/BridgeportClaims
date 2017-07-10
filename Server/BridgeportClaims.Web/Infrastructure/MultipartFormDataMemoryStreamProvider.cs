using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BridgeportClaims.Web.Infrastructure
{
    public class MultipartFormDataMemoryStreamProvider : MultipartMemoryStreamProvider
    {
        private readonly Collection<bool> _isFormData = new Collection<bool>();

        public NameValueCollection FormData { get; } = new NameValueCollection(StringComparer.OrdinalIgnoreCase);

        public Dictionary<string, Stream> FileStreams { get; } = new Dictionary<string, Stream>();

        public override Stream GetStream(HttpContent parent, HttpContentHeaders headers)
        {
            if (null == parent)
                throw new ArgumentNullException(nameof(parent));

            if (null == headers)
                throw new ArgumentNullException(nameof(headers));

            var contentDisposition = headers.ContentDisposition;
            if (null == contentDisposition)
                throw new InvalidOperationException(
                    "Did not find required 'Content-Disposition' header field in MIME multipart body part.");

            _isFormData.Add(string.IsNullOrEmpty(contentDisposition.FileName));
            return base.GetStream(parent, headers);
        }

        public override async Task ExecutePostProcessingAsync()
        {
            for (var index = 0; index < Contents.Count; index++)
            {
                var formContent = Contents[index];
                if (_isFormData[index])
                {
                    // Field
                    var formFieldName = UnquoteToken(formContent.Headers.ContentDisposition.Name) ?? string.Empty;
                    var formFieldValue = await formContent.ReadAsStringAsync();
                    FormData.Add(formFieldName, formFieldValue);
                }
                else
                {
                    // File
                    var fileName = UnquoteToken(formContent.Headers.ContentDisposition.FileName);
                    var stream = await formContent.ReadAsStreamAsync();
                    FileStreams.Add(fileName, stream);
                }
            }
        }

        private static string UnquoteToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return token;
            if (token.StartsWith("\"", StringComparison.Ordinal) && token.EndsWith("\"", StringComparison.Ordinal) &&
                token.Length > 1)
                return token.Substring(1, token.Length - 2);
            return token;
        }
    }
}