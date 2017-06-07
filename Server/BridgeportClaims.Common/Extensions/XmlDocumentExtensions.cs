using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace BridgeportClaims.Common.Extensions
{
    /// <summary>
    /// ToXmlDocument() / ToXDocument() - Type: XDocument or XmlDocument
    /// I can't tell you how many times I've need to convert an XmlDocument 
    /// into an XDocument and vice-versa to use LINQ.These handy extension 
    /// methods will save you a load of time.
    /// </summary>
    public static class XmlDocumentExtensions
    {

        public static XmlDocument ToXmlDocument(this XDocument xDocument)
        {
            var xmlDocument = new XmlDocument();
            using (var xmlReader = xDocument.CreateReader())
            {
                xmlDocument.Load(xmlReader);
            }
            return xmlDocument;
        }
        public static XDocument ToXDocument(this XmlDocument xmlDocument)
        {
            using (var nodeReader = new XmlNodeReader(xmlDocument))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
            }
        }
        public static XmlDocument ToXmlDocument(this XElement xElement)
        {
            var sb = new StringBuilder();
            var xws = new XmlWriterSettings { OmitXmlDeclaration = true, Indent = false };
            using (var xw = XmlWriter.Create(sb, xws))
            {
                xElement.WriteTo(xw);
            }
            var doc = new XmlDocument();
            doc.LoadXml(sb.ToString());
            return doc;
        }
        public static Stream ToMemoryStream(this XmlDocument doc)
        {
            var xmlStream = new MemoryStream();
            doc.Save(xmlStream);
            xmlStream.Flush(); //Adjust this if you want read your data 
            xmlStream.Position = 0;
            return xmlStream;
        }
    }
}
