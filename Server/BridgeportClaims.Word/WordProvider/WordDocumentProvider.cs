using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using BridgeportClaims.Common.Constants;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Word.Enums;
using BridgeportClaims.Word.Templating;
using DocumentFormat.OpenXml.Packaging;

namespace BridgeportClaims.Word.WordProvider
{
    [SuppressMessage("ReSharper", "ImplicitlyCapturedClosure")]
    public class WordDocumentProvider : IWordDocumentProvider
    {
        private readonly Lazy<IWordTemplater> _wordTemplater;

        public WordDocumentProvider(Lazy<IWordTemplater> wordTemplater)
        {
            _wordTemplater = wordTemplater;
        }
        
        public string CreateTemplatedWordDocument(int claimId, string userId, Stream document, LetterType type, int prescriptionId)
        {
            
            var path = Path.GetTempPath();
            var fullFilePath = Path.Combine(path, GetFileName(type));

            // Delete file if it already exists
            if (File.Exists(fullFilePath))
                File.Delete(fullFilePath);
            
            DisposableService.Using(() => new MemoryStream(), ms =>
            {
                var buffer = document.ToBytes();
                ms.Write(buffer, 0, buffer.Length);
                File.WriteAllBytes(fullFilePath, ms.ToArray());
            });
            var docText = string.Empty;
            DisposableService.Using(() => WordprocessingDocument.Open(fullFilePath, true), wordDoc =>
            {
                DisposableService.Using(() => new StreamReader(wordDoc.MainDocumentPart.GetStream()), sr =>
                {
                    docText = sr.ReadToEnd();
                });
                docText = _wordTemplater.Value.TransformDocumentText(claimId, userId, docText, prescriptionId);
                DisposableService.Using(() => new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)), sw =>
                {
                    sw.Write(docText);
                });
            });
            return fullFilePath;
        }

        private string GetFileName(LetterType type)
        {
            string fileName;
            switch (type)
            {
                case LetterType.Ime:
                    fileName = StringConstants.ImeLetterName;
                    break;
                case LetterType.BenExhaust:
                    fileName = StringConstants.BenefitsExhaustedLetter;
                    break;
                case LetterType.PipApp:
                    fileName = StringConstants.PipAppLetter;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            return fileName;
        }
    }
}