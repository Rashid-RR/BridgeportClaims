using System.IO;

namespace BridgeportClaims.Common.FileUploading
{
    public static class StreamProvider
    {
        /// <summary>
        /// Copies the contents of input to output. Doesn't close either stream.
        /// </summary>
        public static void CopyStream(Stream input, Stream output)
        {
            var buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
                output.Write(buffer, 0, len);
        }
    }
}