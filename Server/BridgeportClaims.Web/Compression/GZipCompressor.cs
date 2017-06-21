﻿using System.IO;
using System.IO.Compression;

namespace BridgeportClaims.Web.Compression
{
    public class GZipCompressor : ICompressor
    {

        /// <summary>
        /// The g zip encoding.
        /// </summary>
        private const string GZipEncoding = "gzip";

        /// <summary>
        /// Gets the type of the encoding.
        /// </summary>
        /// <value>
        /// The type of the encoding.
        /// </value>
        public string EncodingType => GZipEncoding;

        /// <summary>
        /// Creates the compression stream.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <returns>
        /// A Compression Stream.
        /// </returns>
        public Stream CreateCompressionStream(Stream output) 
            => new GZipStream(output, CompressionMode.Compress, true);

        /// <summary>
        /// Creates the decompression stream.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        /// A Decompression Stream.
        /// </returns>
        public Stream CreateDecompressionStream(Stream input) 
            => new GZipStream(input, CompressionMode.Decompress, true);
    }
}