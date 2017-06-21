using System.Net.Http;

namespace BridgeportClaims.Web.Compression
{
    public abstract class CompressionHttpContent : HttpContent
    {
        /// <summary>
        /// The original content.
        /// </summary>
        private readonly HttpContent _originalContent;

        /// <summary>
        /// The compressor.
        /// </summary>
        private readonly ICompressor _compressor;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompressionHttpContent"/> class.
        /// </summary>
        /// <param name="content">The original HttpContent object.</param>
        /// <param name="compressor">The compressor.</param>
        public CompressionHttpContent(HttpContent content, ICompressor compressor)
        {
            _originalContent = content;
            _compressor = compressor;

            SetContentHeaders();
        }

        /// <summary>
        /// Gets the <see cref="ICompressor"/>.
        /// </summary>
        /// <value>
        /// The compressor.
        /// </value>
        protected ICompressor Compressor => _compressor;

        /// <summary>
        /// Gets the original <see cref="HttpContent"/>.
        /// </summary>
        /// <value>
        /// The original <see cref="HttpContent"/>.
        /// </value>
        protected HttpContent OriginalContent => _originalContent;

        /// <summary>
        /// Determines whether the HTTP content has a valid length in bytes.
        /// </summary>
        /// <param name="length">The length in bytes of the HHTP content.</param>
        /// <returns>
        /// Returns <see cref="T:System.Boolean" />.true if <paramref name="length" /> is a valid length; otherwise, false.
        /// </returns>
        protected override bool TryComputeLength(out long length)
        {
            length = -1;
            return false;
        }

        /// <summary>
        /// The set content headers.
        /// </summary>
        private void SetContentHeaders()
        {
            //// copy headers from original content
            foreach (var header in _originalContent.Headers)
            {
                Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            //// add the content encoding header
            Headers.ContentEncoding.Add(_compressor.EncodingType);
        }
    }
}