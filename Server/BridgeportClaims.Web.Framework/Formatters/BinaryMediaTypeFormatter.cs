using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BridgeportClaims.Web.Framework.Formatters
{
    public class BinaryMediaTypeFormatter : MediaTypeFormatter
    {
        private static readonly Type SupportedType = typeof(byte[]);

        public BinaryMediaTypeFormatter() : this(false) { }

        public BinaryMediaTypeFormatter(bool isAsync)
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/octet-stream"));
            IsAsync = isAsync;
        }

        public bool IsAsync { get; set; } = false;


        public override bool CanReadType(Type type)
        {
            return type == SupportedType;
        }

        public override bool CanWriteType(Type type)
        {
            return type == SupportedType;
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream stream,
            HttpContent httpContent, IFormatterLogger formatterLogger)
        {
            var readTask = GetReadTask(stream);
            if (IsAsync)
            {
                readTask.Start();
            }
            else
            {
                readTask.RunSynchronously();
            }
            return readTask;

        }

        private static Task<object> GetReadTask(Stream stream)
        {
            return new Task<object>(() =>
            {
                var ms = new MemoryStream();
                stream.CopyTo(ms);
                return ms.ToArray();
            });
        }

        private Task GetWriteTask(Stream stream, byte[] data)
        {
            return new Task(() =>
            {
                var ms = new MemoryStream(data);
                ms.CopyTo(stream);
            });
        }


        public override Task WriteToStreamAsync(Type type, object value, Stream stream,
            HttpContent contentHeaders, TransportContext transportContext)
        {

            if (value == null)
                value = new byte[0];
            var writeTask = GetWriteTask(stream, (byte[]) value);
            if (IsAsync)
            {
                writeTask.Start();
            }
            else
            {
                writeTask.RunSynchronously();
            }
            return writeTask;
        }
    }
}