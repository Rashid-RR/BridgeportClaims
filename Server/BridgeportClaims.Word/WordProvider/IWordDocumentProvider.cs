using System.IO;
using BridgeportClaims.Word.Enums;

namespace BridgeportClaims.Word.WordProvider
{
    public interface IWordDocumentProvider
    {
        string CreateTemplatedWordDocument(int claimId, string userId, Stream document, LetterType type, int prescriptionId);
    }
}