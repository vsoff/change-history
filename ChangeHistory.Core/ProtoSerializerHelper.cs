using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ChangeHistory.Core
{
    public static class ProtoSerializerHelper
    {
        public static byte[] Serialize<T>(T data)
        {
            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, data);
                return stream.ToArray();
            }
        }

        public static T Deserialize<T>(byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            {
                return Serializer.Deserialize<T>(stream);
            }
        }

        public static object Deserialize(Type type, byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            {
                return Serializer.Deserialize(type, stream);
            }
        }
    }
}
