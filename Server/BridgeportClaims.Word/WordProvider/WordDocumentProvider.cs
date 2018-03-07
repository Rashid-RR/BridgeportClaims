using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Word.Enums;
using BridgeportClaims.Word.Templating;
using DocumentFormat.OpenXml.Packaging;
using c = BridgeportClaims.Common.StringConstants.Constants;

namespace BridgeportClaims.Word.WordProvider
{
    [SuppressMessage("ReSharper", "ImplicitlyCapturedClosure")]
    public class WordDocumentProvider : IWordDocumentProvider
    {
        private readonly IWordTemplater _wordTemplater;

        public WordDocumentProvider(IWordTemplater wordTemplater)
        {
            _wordTemplater = wordTemplater;
        }
        
        public string CreateTemplatedWordDocument(int claimId, string userId, Stream document, LetterType type)
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
                docText = _wordTemplater.TransformDocumentText(claimId, userId, docText);
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
                    fileName = c.ImeLetterName;
                    break;
                case LetterType.BenExhaust:
                    fileName = c.BenefitsExhaustedLetter;
                    break;
                case LetterType.PipApp:
                    fileName = c.PipAppLetter;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            return fileName;
        }
    }
}