using System;
using System.Globalization;
using System.Text.RegularExpressions;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.LetterGenerations;

namespace BridgeportClaims.Word.Templating
{
    public class WordTemplater : IWordTemplater
    {
        private readonly ILetterGenerationProvider _letterGenerationProvider;

        public WordTemplater(ILetterGenerationProvider letterGenerationProvider)
        {
            _letterGenerationProvider = letterGenerationProvider;
        }

        public string TransformDocumentText(int claimId, string userId, string docText, int prescriptionId)
        {
            var data = _letterGenerationProvider.GetLetterGenerationData(claimId, userId, prescriptionId);
            var ti = new CultureInfo("en-US", false).TextInfo;
            if (null == data)
                throw new ArgumentNullException(nameof(data));
            var r1 = new Regex("Patient.FirstName");
            docText = r1.Replace(docText,
                data.FirstName.IsNotNullOrWhiteSpace()
                    ? ti.ToTitleCase(ti.ToLower(data.FirstName))
                    : string.Empty);
            var r2 = new Regex("MM/DD/YYYY");
            docText = r2.Replace(docText,
                data.TodaysDate.IsNotNullOrWhiteSpace() ? data.TodaysDate : DateTime.Now.ToString("d"));
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
                    ? ti.ToTitleCase(ti.ToLower(data.City))
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
                data.GroupName.IsNotNullOrWhiteSpace()
                    ? ti.ToTitleCase(ti.ToLower(data.GroupName))
                    : string.Empty);
            var r10 = new Regex("AspNetUsers.FirstName");
            docText = r10.Replace(docText,
                data.UserFirstName.IsNotNullOrWhiteSpace()
                    ? ti.ToTitleCase(ti.ToLower(data.UserFirstName))
                    : string.Empty);
            var r11 = new Regex("Asp.NetUsers.LastName");
            docText = r11.Replace(docText, data.UserLastName.IsNotNullOrWhiteSpace()
                    ? ti.ToTitleCase(ti.ToLower(data.UserLastName)) : string.Empty);
            return docText;
        }
    }
}