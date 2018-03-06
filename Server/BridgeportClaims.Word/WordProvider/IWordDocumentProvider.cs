using System.IO;

namespace BridgeportClaims.Word.WordProvider
{
    public interface IWordDocumentProvider
    {
        string CreateTemplatedWordDocument(Stream document);
    }
}