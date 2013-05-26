using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace ClassLibraryHelper.C_
{
    public class BinarySerializer
    {
        //将类型序列化字符串
        public static string Serializer<T>(T t)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter serializer = new BinaryFormatter();
                serializer.Serialize(stream, t);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        //将类型序列化文件
        public static void SerializerToFile<T>(T t, string path, string fullName)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var fullPath = string.Format(@"{0}\{1}", path, fullName);
            using (FileStream stream = new FileStream(fullPath, FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, t);
                stream.Flush();
            }
        }

        //将字符串反序列化类型
        public static T Deserializer<T>(string str) where T : class
        {
            byte[] bs = Encoding.UTF8.GetBytes(str);
            using (MemoryStream stream = new MemoryStream(bs))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(stream) as T;
            }
        }

        //将文件反序列化类型
        public static T DeserializerFromFile<T>(string path) where T : class
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(stream) as T;
            }
        }
    }
}
