using System;
using System.IO;
using System.Reflection;
using BridgeportClaims.Word.Enums;
using BridgeportClaims.Word.WordProvider;
using c = BridgeportClaims.Common.StringConstants.Constants;

namespace BridgeportClaims.Word.FileDriver
{
    public class WordFileDriver : IWordFileDriver
    {
        private readonly Lazy<IWordDocumentProvider> _wordDocumentProvider;

        public WordFileDriver(Lazy<IWordDocumentProvider> wordDocumentProvider)
        {
            _wordDocumentProvider = wordDocumentProvider;
        }

        private static Stream GetManifestResourcStream(LetterType type)
        {
            string resourceString;
            switch (type)
            {
                case LetterType.Ime:
                    resourceString = c.ImeLetterManifestResource;
                    break;
                case LetterType.BenExhaust:
                    resourceString = c.BenefitsExhaustedLetterManifestResource;
                    break;
                case LetterType.PipApp:
                    resourceString = c.PipAppLetterManifestResource;
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
            var path = _wordDocumentProvider.Value.CreateTemplatedWordDocument(claimId, userId, GetManifestResourcStream(type), type, prescriptionId);
            return path;
        }
    }
}