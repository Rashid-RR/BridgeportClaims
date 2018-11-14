using System.Collections.Generic;
using System.IO;
using BridgeportClaims.Word.Enums;

namespace BridgeportClaims.Word.WordProvider
{
    public interface IWordDocumentProvider
    {
        string CreateTemplateWordDocument(int claimId, string userId, Stream document, LetterType type, int prescriptionId);
        string CreateDrNoteTemplateWordDocument(int claimId, string userId, Stream document, int firstPrescriptionId,
            IEnumerable<int> prescriptionIds);
    }
}