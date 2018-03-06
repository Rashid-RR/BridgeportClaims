using System.Text.RegularExpressions;

namespace BridgeportClaims.Word.Templating
{
    public class WordTemplater
    {
        public static string TransformDocumentText(string docText)
        {
            var regexText = new Regex("Patient.FirstName");
            docText = regexText.Replace(docText, "Joe");
            return docText;
        }
    }
}