using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.RegularExpressions;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Word.Templating;
using DocumentFormat.OpenXml.Packaging;

namespace BridgeportClaims.Word.WordProvider
{
    [SuppressMessage("ReSharper", "ImplicitlyCapturedClosure")]
    public class WordDocumentProvider : IWordDocumentProvider
    {
        // TODO: Handle templating of each letter type.
        public string CreateTemplatedWordDocument(Stream document)
        {
            const string fileName = "IME Letter.docx";
            var path = Path.GetTempPath();
            var fullFilePath = Path.Combine(path, fileName);

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
                docText = WordTemplater.TransformDocumentText(docText);
                DisposableService.Using(() => new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)), sw =>
                {
                    sw.Write(docText);
                });
            });
            return fullFilePath;
        }
    }
}