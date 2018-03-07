using System;
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

        public string TransformDocumentText(int claimId, string userId, string docText)
        {
            var data = _letterGenerationProvider.GetLetterGenerationData(claimId, userId);
            var r1 = new Regex("Patient.FirstName");
            docText = r1.Replace(docText, data.FirstName);
            var r2 = new Regex("MM/DD/YYYY");
            docText = r2.Replace(docText, data.TodaysDate);
            var r3 = new Regex("Patient.LastName");
            docText = r3.Replace(docText, data.LastName);
            var r4 = new Regex("Patient.Address1");
            docText = r4.Replace(docText, data.Address1);
            if (data.Address2.IsNotNullOrWhiteSpace())
            {
                docText = docText + Environment.NewLine + data.Address2;
            }
            var r5 = new Regex("Patient.City");
            docText = r5.Replace(docText, data.City);
            var r6 = new Regex("UsState.StateCode");
            docText = r6.Replace(docText, data.StateCode);
            var r7 = new Regex("Patient.PostalCode");
            docText = r7.Replace(docText, data.PostalCode);
            var r8 = new Regex("Pharmacy.PharmacyName");
            docText = r8.Replace(docText, data.PharmacyName);
            var r9 = new Regex("Payor.GroupName");
            docText = r9.Replace(docText, data.GroupName);
            var r10 = new Regex("AspNetUsers.FirstName");
            docText = r10.Replace(docText, data.UserFirstName);
            var r11 = new Regex("Asp.NetUsers.LastName");
            docText = r11.Replace(docText, data.UserLastName);
            return docText;
        }
    }
}