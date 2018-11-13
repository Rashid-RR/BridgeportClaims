using System;
using System.Data;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.LetterGenerations;

namespace BridgeportClaims.Word.Templating
{
    public class WordTemplate : IWordTemplate
    {
        private readonly Lazy<ILetterGenerationProvider> _letterGenerationProvider;

        public WordTemplate(Lazy<ILetterGenerationProvider> letterGenerationProvider)
        {
            _letterGenerationProvider = letterGenerationProvider;
        }

        // TODO: Lot of duplication here too, although much harder to avoid.
        public string TransformDrNoteDocumentText(int claimId, string userId, string docText, int firstPrescriptionId,
            DataTable dt)
        {
            var data = _letterGenerationProvider.Value.GetDrNoteLetterGenerationData(claimId, userId,
                firstPrescriptionId, dt);
            if (null == data)
            {
                throw new Exception("Could not find information to fill the document from the database.");
            }
            var result = data.Result;
            var ti = new CultureInfo("en-US", false).TextInfo;
            if (null == data)
                throw new ArgumentNullException(nameof(data));
            var prescriberFirstName = string.Empty;
            var prescriberLastName = string.Empty;
            if (result.PrescriberName.IsNotNullOrWhiteSpace())
            {
                const string delimiter = ", ";
                var prescriberNames = result.PrescriberName.Split(new[] { delimiter }, StringSplitOptions.None);
                prescriberFirstName = prescriberNames[1];
                prescriberLastName = prescriberNames[0];
            }
            var r1 = new Regex("MM/DD/YYYY");
            docText = r1.Replace(docText, $"{result.TodaysDate:MMMM dd, yyyy}");
            var r2 = new Regex("Prescription.Prescriber");
            docText = r2.Replace(docText,
                result.PrescriberName.IsNotNullOrWhiteSpace()
                    ? "Dr. " + ti.ToTitleCase(ti.ToLower(prescriberFirstName)) + " " +
                      ti.ToTitleCase(ti.ToLower(prescriberLastName))
                    : string.Empty);
            var r3 = new Regex("Prescriber.Address1");
            docText = r3.Replace(docText,
                result.Addr1.IsNotNullOrWhiteSpace()
                    ? ti.ToTitleCase(ti.ToLower(result.Addr1)) +
                      (result.Addr2.IsNotNullOrWhiteSpace()
                          ? "</w:t><w:br/><w:t>" + ti.ToTitleCase(ti.ToLower(result.Addr2))
                          : string.Empty)
                    : string.Empty);
            var r4 = new Regex("Prescriber.City");
            docText = r4.Replace(docText,
                result.City.IsNotNullOrWhiteSpace()
                    ? ti.ToTitleCase(ti.ToLower(result.City)) + ","
                    : string.Empty);
            var r5 = new Regex("UsState.StateCode");
            docText = r5.Replace(docText,
                result.StateCode.IsNotNullOrWhiteSpace() ? result.StateCode.ToUpper() : string.Empty);
            var r6 = new Regex("Prescriber.PostalCode");
            docText = r6.Replace(docText,
                result.PostalCode.IsNotNullOrWhiteSpace() ? result.PostalCode.ToUpper() : string.Empty);
            var r7 = new Regex("Patient.FirstName");
            docText = r7.Replace(docText,
                result.FirstName.IsNotNullOrWhiteSpace()
                    ? ti.ToTitleCase(ti.ToLower(result.FirstName))
                    : string.Empty);
            var r8 = new Regex("Patient.LastName");
            docText = r8.Replace(docText,
                result.LastName.IsNotNullOrWhiteSpace()
                    ? ti.ToTitleCase(ti.ToLower(result.LastName))
                    : string.Empty);
            var r9 = new Regex("Patient.DateOfBirth");
            docText = r9.Replace(docText,
                null != result.DateOfBirth ? result.DateOfBirth.Value.ToString("d") : string.Empty);
            var r10 = new Regex("Claim.ClaimNumber");
            docText = r10.Replace(docText,
                result.ClaimNumber.IsNotNullOrWhiteSpace() ? result.ClaimNumber : string.Empty);
            var r11 = new Regex("Prescriber.LastName");
            docText = r11.Replace(docText,
                prescriberLastName.IsNotNullOrWhiteSpace()
                    ? ti.ToTitleCase(ti.ToLower(prescriberLastName))
                    : string.Empty);
            var r12 = new Regex("Pharmacy.PharmacyName");
            docText = r12.Replace(docText,
                result.PharmacyName.IsNotNullOrWhiteSpace()
                    ? ti.ToTitleCase(ti.ToLower(result.PharmacyName))
                    : string.Empty);
            var r13 = new Regex("Payor.LetterName");
            docText = r13.Replace(docText,
                result.LetterName.IsNotNullOrWhiteSpace() ? result.LetterName : string.Empty);
            var r14 = new Regex("Prescription.Plurality");
            docText = r14.Replace(docText, result.Plurality);
            var r15 = new Regex("AspNetUsers.FirstName");
            docText = r15.Replace(docText,
                result.UserFirstName.IsNotNullOrWhiteSpace()
                    ? ti.ToTitleCase(ti.ToLower(result.UserFirstName))
                    : string.Empty);
            var r16 = new Regex("Asp.NetUsers.LastName");
            docText = r16.Replace(docText, result.UserLastName.IsNotNullOrWhiteSpace()
                ? ti.ToTitleCase(ti.ToLower(result.UserLastName)) : string.Empty);
            var r17 = new Regex("AspNetUsers.Extension");
            docText = r17.Replace(docText, result.Extension.IsNotNullOrWhiteSpace()
                ? "x " + ti.ToTitleCase(ti.ToLower(result.Extension)) : string.Empty);
            // Here comes the fun - the list of scripts.
            var r18 = new Regex("Prescription.LabelName");
            var sb = new StringBuilder();
            const string bulletItem = "<w:p><w:pPr><w:pStyle w:val=\"ListParagraph\"/><w:numPr><w:ilvl w:val =\"0\"/><w:numId w:val =\"1\"/></w:numPr></w:pPr><w:r><w:t>{0} dispensed on {1}</w:t></w:r></w:p>";
            foreach (var script in data.Scripts)
            {
                sb.Append(string.Format(bulletItem, script.LabelName, script.DateFilled.ToString("d")));
            }
            docText = r18.Replace(docText, sb.ToString());
            var r19 = new Regex("IsArePlurality");
            docText = r19.Replace(docText, result.Plurality == "s" ? "are" : "is");
            return docText;
        }

        public string TransformDocumentText(int claimId, string userId, string docText, int prescriptionId)
        {
            var data = _letterGenerationProvider.Value.GetLetterGenerationData(claimId, userId, prescriptionId);
            var ti = new CultureInfo("en-US", false).TextInfo;
            if (null == data)
                throw new ArgumentNullException(nameof(data));
            var r1 = new Regex("Patient.FirstName");
            docText = r1.Replace(docText,
                data.FirstName.IsNotNullOrWhiteSpace()
                    ? ti.ToTitleCase(ti.ToLower(data.FirstName))
                    : string.Empty);
            var r2 = new Regex("MM/DD/YYYY");
            docText = r2.Replace(docText, $"{data.TodaysDate:MMMM dd, yyyy}");
            var r3 = new Regex("Patient.LastName");
            docText = r3.Replace(docText,
                data.LastName.IsNotNullOrWhiteSpace()
                    ? ti.ToTitleCase(ti.ToLower(data.LastName))
                    : string.Empty);
            var r4 = new Regex("Patient.Address1");
            docText = r4.Replace(docText,
                data.Address1.IsNotNullOrWhiteSpace()
                    ? ti.ToTitleCase(ti.ToLower(data.Address1)) +
                      (data.Address2.IsNotNullOrWhiteSpace()
                          ? "</w:t><w:br/><w:t>" + ti.ToTitleCase(ti.ToLower(data.Address2))
                          : string.Empty)
                    : string.Empty);
            var r5 = new Regex("Patient.City");
            docText = r5.Replace(docText,
                data.City.IsNotNullOrWhiteSpace()
                    ? ti.ToTitleCase(ti.ToLower(data.City)) + ","
                    : string.Empty);
            var r6 = new Regex("UsState.StateCode");
            docText = r6.Replace(docText,
                data.StateCode.IsNotNullOrWhiteSpace() ? data.StateCode.ToUpper() : string.Empty);
            var r7 = new Regex("Patient.PostalCode");
            docText = r7.Replace(docText,
                data.PostalCode.IsNotNullOrWhiteSpace() ? data.PostalCode.ToUpper() : string.Empty);
            var r8 = new Regex("Pharmacy.PharmacyName");
            docText = r8.Replace(docText,
                data.PharmacyName.IsNotNullOrWhiteSpace()
                    ? ti.ToTitleCase(ti.ToLower(data.PharmacyName))
                    : string.Empty);
            var r9 = new Regex("Payor.GroupName");
            docText = r9.Replace(docText,
                data.LetterName.IsNotNullOrWhiteSpace() ? data.LetterName : string.Empty);
            var r10 = new Regex("AspNetUsers.FirstName");
            docText = r10.Replace(docText,
                data.UserFirstName.IsNotNullOrWhiteSpace()
                    ? ti.ToTitleCase(ti.ToLower(data.UserFirstName))
                    : string.Empty);
            var r11 = new Regex("Asp.NetUsers.LastName");
            docText = r11.Replace(docText, data.UserLastName.IsNotNullOrWhiteSpace()
                    ? ti.ToTitleCase(ti.ToLower(data.UserLastName)) : string.Empty);
            var r12 = new Regex("AspNetUsers.Extension");
            docText = r12.Replace(docText, data.Extension.IsNotNullOrWhiteSpace()
                    ? "x " + ti.ToTitleCase(ti.ToLower(data.Extension)) : string.Empty);
            return docText;
        }
    }
}