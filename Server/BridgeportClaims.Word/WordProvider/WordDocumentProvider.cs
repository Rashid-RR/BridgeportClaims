using System.IO;
using System.Text.RegularExpressions;
using BridgeportClaims.Common.Extensions;
using DocumentFormat.OpenXml.Packaging;

namespace BridgeportClaims.Word.WordProvider
{
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

            var buffer = document.ToBytes();
            using (var ms = new MemoryStream())
            {
                ms.Write(buffer, 0, buffer.Length);
                File.WriteAllBytes(fullFilePath, ms.ToArray());
            }
            using (var wordDoc = WordprocessingDocument.Open(fullFilePath, true))
            {
                string docText = null;
                using (var sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }

                var regexText = new Regex("Patient.FirstName");
                docText = regexText.Replace(docText, "Joe");

                using (var sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }
            }
            return fullFilePath;
        }
    }
}