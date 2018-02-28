using Microsoft.Office.Interop.Word;

namespace BridgeportClaims.Word.Factories
{
    public class WordFactory
    {
        public string GenerateWordDocument()
        {
            var wordApp = new Application {Visible = true};
            var wordDoc = wordApp.Documents.Open(@"C:\test.docx");
            var bkm = wordDoc.Bookmarks["name_field"];
            var rng = bkm.Range;
            rng.Text = "Adams Laura"; //Get value from any where
            return rng.Text;
        }
    }
}