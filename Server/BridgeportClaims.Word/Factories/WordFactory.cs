using System.IO;
using Microsoft.Office.Interop.Word;

namespace BridgeportClaims.Word.Factories
{
    public class WordFactory
    {
        public string GetWordDoc()
        {
            Application wordApp = null;
            wordApp = new Application {Visible = true};
            wordApp = new Application {Visible = true};
            Document wordDoc = wordApp.Documents.Open(@"C:\test.docx");
            Bookmark bkm = wordDoc.Bookmarks["name_field"];
            var rng = bkm.Range;
            rng.Text = "Adams Laura"; //Get value from any where
            //Remember to properly save & close the document.(You can see this)
            var tempPath = Path.GetTempPath();
            var fileName = "test.docx";
            return Path.Combine(tempPath, fileName);
        }
    }
}