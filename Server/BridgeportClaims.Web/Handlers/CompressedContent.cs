using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BridgeportClaims.Web.Handlers
{
    public class CompressedContent : HttpContent
    {
        private readonly HttpContent _originalContent;
        private readonly string _encodingType;

        public CompressedContent(HttpContent content, string encodingType)
        {
            _originalContent = content ?? throw new ArgumentNullException(nameof(content));
            this._encodingType = encodingType?.ToLowerInvariant() ?? throw new ArgumentNullException(nameof(encodingType));

            if (this._encodingType != "gzip" && this._encodingType != "deflate")
            {
                throw new InvalidOperationException(
                    $"Encoding '{_encodingType}' is not supported. Only supports gzip or deflate encoding.");
            }

            // copy the headers from the original content
            foreach (var header in _originalContent.Headers)
                Headers.TryAddWithoutValidation(header.Key, header.Value);

            Headers.ContentEncoding.Add(encodingType);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = -1;

            return false;
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            Stream compressedStream = null;

            switch (_encodingType)
            {
                case "gzip":
                    compressedStream = new GZipStream(stream, CompressionMode.Compress, leaveOpen: true);
                    break;
                case "deflate":
                    compressedStream = new DeflateStream(stream, CompressionMode.Compress, leaveOpen: true);
                    break;
            }

            return _originalContent.CopyToAsync(compressedStream).ContinueWith(tsk =>
            {
                compressedStream?.Dispose();
            });
        }
    }
}