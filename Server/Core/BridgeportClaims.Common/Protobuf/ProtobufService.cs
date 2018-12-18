using System;
using System.IO;
using BridgeportClaims.Common.Disposable;
using NLog;
using ProtoBuf;

namespace BridgeportClaims.Common.Protobuf
{
    public static class ProtobufService
    {
        private static readonly Lazy<ILogger> Logger = new Lazy<ILogger>(LogManager.GetCurrentClassLogger);

        public static byte[] ProtoSerialize<T>(T record) where T : class
        {
            if (null == record)
            {
                return null;
            }
            try
            {
                return DisposableService.Using(() => new MemoryStream(), stream =>
                {
                    Serializer.Serialize(stream, record);
                    return stream?.ToArray();
                });
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                throw;
            }
        }

        public static T ProtoDeserialize<T>(byte[] data) where T : class
        {
            if (null == data)
            {
                return null;
            }
            try
            {
                return DisposableService.Using(() => new MemoryStream(data),
                    Serializer.Deserialize<T>);
            }
            catch (Exception ex)
            {
                Logger.Value.Error(ex);
                throw;
            }
        }
    }
}