using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using s = BridgeportClaims.Common.Constants.StringConstants;
using BridgeportClaims.Word.Enums;
using BridgeportClaims.Word.WordProvider;

namespace BridgeportClaims.Word.FileDriver
{
    public class WordFileDriver : IWordFileDriver
    {
        private readonly Lazy<IWordDocumentProvider> _wordDocumentProvider;

        public WordFileDriver(Lazy<IWordDocumentProvider> wordDocumentProvider)
        {
            _wordDocumentProvider = wordDocumentProvider;
        }

        private static Stream GetManifestResourceStream(LetterType type)
        {
            string resourceString;
            switch (type)
            {
                case LetterType.Ime:
                    resourceString = s.ImeLetterManifestResource;
                    break;
                case LetterType.BenExhaust:
                    resourceString = s.BenefitsExhaustedLetterManifestResource;
                    break;
                case LetterType.PipApp:
                    resourceString = s.PipAppLetterManifestResource;
                    break;
                case LetterType.Denial:
                    resourceString = s.DenialLetterManifestResource;
                    break;
                case LetterType.UnderInvestigation:
                    resourceString = s.UnderInvestigationManifestResource;
                    break;
                case LetterType.DrNoteLetter:
                    resourceString = s.DrNoteLetterManifestResource;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream(resourceString);
            return stream;
        }

        public string GetLetterByType(int claimId, string userId, LetterType type, int prescriptionId)
        {
            var path = _wordDocumentProvider.Value.CreateTemplateWordDocument(claimId, userId, GetManifestResourceStream(type), type, prescriptionId);
            return path;
        }

        public string GetDrLetter(int claimId, int firstPrescriptionId, IEnumerable<int> prescriptionIds, string userId)
        {
            var path =
                _wordDocumentProvider.Value.CreateDrNoteTemplateWordDocument(claimId, userId,
                    GetManifestResourceStream(LetterType.DrNoteLetter)
                    , firstPrescriptionId, prescriptionIds);
            return path;
        }
    }
}