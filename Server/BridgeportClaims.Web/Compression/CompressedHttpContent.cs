using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BridgeportClaims.Web.Compression
{
    public class CompressedHttpContent : CompressionHttpContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompressedHttpContent"/> class.
        /// </summary>
        /// <param name="content">The original HttpContent object.</param>
        /// <param name="compressor">The compressor.</param>
        public CompressedHttpContent(HttpContent content, ICompressor compressor)
            : base(content, compressor)
        {
        }

        /// <summary>
        /// Serialize the HTTP content to a stream as an asynchronous operation.
        /// </summary>
        /// <param name="stream">The target stream.</param>
        /// <param name="context">Information about the transport (channel binding token, for example). This parameter may be null.</param>
        /// <returns>
        /// Returns <see cref="T:System.Threading.Tasks.Task" />.The task object representing the asynchronous operation.
        /// </returns>
        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            var compressionStream = Compressor.CreateCompressionStream(stream);
            return OriginalContent.CopyToAsync(compressionStream).ContinueWith(task =>
            {
                compressionStream?.Dispose();
            });
        }
    }
}