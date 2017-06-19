using System.IO;

namespace BridgeportClaims.Web.Compression
{
    public interface ICompressor
    {
        string EncodingType { get; }
        Stream CreateCompressionStream(Stream output);
        Stream CreateDecompressionStream(Stream input);
    }
}