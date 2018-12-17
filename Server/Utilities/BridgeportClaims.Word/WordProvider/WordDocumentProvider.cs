using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.Prescriptions;
using BridgeportClaims.Word.Enums;
using BridgeportClaims.Word.Templating;
//using DocumentFormat.OpenXml.Packaging;
using s = BridgeportClaims.Common.Constants.StringConstants;

namespace BridgeportClaims.Word.WordProvider
{
    [SuppressMessage("ReSharper", "ImplicitlyCapturedClosure")]
    public class WordDocumentProvider : IWordDocumentProvider
    {
        private readonly Lazy<IWordTemplate> _wordTemplate;
        private readonly Lazy<IPrescriptionsDataProvider> _prescriptionsDataProvider;

        public WordDocumentProvider(Lazy<IWordTemplate> wordTemplate,
            Lazy<IPrescriptionsDataProvider> prescriptionsDataProvider)
        {
            _wordTemplate = wordTemplate;
            _prescriptionsDataProvider = prescriptionsDataProvider;
        }

        /// <summary>
        /// The best method in this class so far that implements the DRY method.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        private string GetFullFilePathFromStreamAndType(LetterType type, Stream document)
        {
            var path = Path.GetTempPath();
            var fullFilePath = Path.Combine(path, GetFileName(type));

            // Delete file if it already exists
            if (File.Exists(fullFilePath))
            {
                File.Delete(fullFilePath);
            }
            DisposableService.Using(() => new MemoryStream(), ms =>
            {
                var buffer = document.ToBytes();
                ms.Write(buffer, 0, buffer.Length);
                File.WriteAllBytes(fullFilePath, ms.ToArray());
            });
            return fullFilePath;
        }

        public string CreateTemplateWordDocument(int claimId, string userId, Stream document, LetterType type, int prescriptionId)
        {
            var fullFilePath = GetFullFilePathFromStreamAndType(type, document);
            /*var docText = string.Empty;
            DisposableService.Using(() => WordprocessingDocument.Open(fullFilePath, true), wordDoc =>
            {
                DisposableService.Using(() => new StreamReader(wordDoc.MainDocumentPart.GetStream()), sr =>
                {
                    docText = sr.ReadToEnd();
                });
                docText = _wordTemplate.Value.TransformDocumentText(claimId, userId, docText, prescriptionId);
                DisposableService.Using(() => new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)), sw =>
                {
                    sw.Write(docText);
                });
            });*/
            return fullFilePath;
        }


        // TODO: Refactor to avoid duplicating all this code from method above.
        public string CreateDrNoteTemplateWordDocument(int claimId, string userId, Stream document, int firstPrescriptionId, IEnumerable<int> prescriptionIds)
        {
            var fullFilePath = GetFullFilePathFromStreamAndType(LetterType.DrNoteLetter, document);
            /*var docText = string.Empty;
            DisposableService.Using(() => WordprocessingDocument.Open(fullFilePath, true), wordDoc =>
            {
                DisposableService.Using(() => new StreamReader(wordDoc.MainDocumentPart.GetStream()), sr =>
                {
                    docText = sr.ReadToEnd();
                });
                IList<PrescriptionIdDto> dto = new List<PrescriptionIdDto>();
                prescriptionIds.ForEach(x => dto.Add(_prescriptionsDataProvider.Value.GetPrescriptionIdDto(x)));
                var dt = dto.ToFixedDataTable();
                docText = _wordTemplate.Value.TransformDrNoteDocumentText(claimId, userId, docText, firstPrescriptionId, dt);
                DisposableService.Using(() => new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)), sw =>
                {
                    sw.Write(docText);
                });
            });*/
            return fullFilePath;
        }

        private string GetFileName(LetterType type)
        {
            string fileName;
            switch (type)
            {
                case LetterType.Ime:
                    fileName = s.ImeLetterName;
                    break;
                case LetterType.BenExhaust:
                    fileName = s.BenefitsExhaustedLetter;
                    break;
                case LetterType.PipApp:
                    fileName = s.PipAppLetter;
                    break;
                case LetterType.Denial:
                    fileName = s.DenialLetterName;
                    break;
                case LetterType.UnderInvestigation:
                    fileName = s.UnderInvestigationLetterName;
                    break;
                case LetterType.DrNoteLetter:
                    fileName = s.DrLetterName;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            return fileName;
        }
    }
}