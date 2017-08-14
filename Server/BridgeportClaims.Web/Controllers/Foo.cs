using System;
using System.Web.DynamicData;

namespace BridgeportClaims.Web.Controllers
{
    public class Foo : IFieldFormattingOptions
    {
        public void DoIt()
        {
            try
            {
                // TODO: somethign new 
                var a = string.Empty;
                a = $"Hello World{a}!";
                /*
                 I am a comment here l I i
                 Hieie
                 */
            }
            finally
            {
                // TODO:
            }
        }

        public bool ApplyFormatInEditMode { get; }
        public string DataFormatString { get; }
        public bool ConvertEmptyStringToNull { get; }
        public string NullDisplayText { get; }
        public bool HtmlEncode { get; }
    }


}