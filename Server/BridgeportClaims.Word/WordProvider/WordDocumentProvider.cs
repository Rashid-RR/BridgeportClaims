using NLog;
using System;
using System.IO;
using Microsoft.Office.Interop.Word;
using c = BridgeportClaims.Common.StringConstants.Constants;

namespace BridgeportClaims.Word.WordProvider
{
    public class WordDocumentProvider : IWordDocumentProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private const string FileName = c.ImeLetterName; // TODO: Embedded Resource.
        private readonly string _filePath = $@"C:\Users\Public\{FileName}";

        public string GetWordDocument()
        {
            var ap = new Application();
            try
            {
                var doc = ap.Documents.Open(_filePath, ReadOnly: false, Visible: false);
                doc.Activate();
                var sel = ap.Selection;
                if (null != sel)
                {
                    switch (sel.Type)
                    {
                        case WdSelectionType.wdSelectionIP:
                            sel.TypeText(DateTime.Now.ToShortDateString());
                            sel.TypeParagraph();
                            break;

                        case WdSelectionType.wdNoSelection:
                            break;
                        case WdSelectionType.wdSelectionNormal:
                            break;
                        case WdSelectionType.wdSelectionFrame:
                            break;
                        case WdSelectionType.wdSelectionColumn:
                            break;
                        case WdSelectionType.wdSelectionRow:
                            break;
                        case WdSelectionType.wdSelectionBlock:
                            break;
                        case WdSelectionType.wdSelectionInlineShape:
                            break;
                        case WdSelectionType.wdSelectionShape:
                            break;
                        default:
                            Console.WriteLine("Selection type not handled; no writing done");
                            break;

                    }

                    // Remove all meta data.
                    doc.RemoveDocumentInformation(WdRemoveDocInfoType.wdRDIAll);

                    ap.Documents.Save(true, true);
                }
                else
                {
                    Console.WriteLine("Unable to acquire Selection...no writing to document done..");
                }
                
                var path = Path.GetTempPath();
                var fullFilePath = Path.Combine(path, FileName);
                ap.ActiveDocument.SaveAs(fullFilePath);
                return fullFilePath;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
            finally
            {
                // Ambiguity between method 'Microsoft.Office.Interop.Word._Application.Quit(ref object, ref object, ref object)' and non-method 'Microsoft.Office.Interop.Word.ApplicationEvents4_Event.Quit'. Using method group.
                // ap.Quit( SaveChanges: false, OriginalFormat: false, RouteDocument: false );
                ((_Application)ap).Quit(false, false, false);

                System.Runtime.InteropServices.Marshal.ReleaseComObject(ap);
            }
        }
    }
}