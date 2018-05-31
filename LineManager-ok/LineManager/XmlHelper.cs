using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace LineManager
{
    public class XmlHelper
    {
        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <typeparam name="T"> 对象类型 </typeparam>
        /// <param name="t"> 对象 </param>
        /// <returns></returns>
        public static string Serialize<T>(T t)
        {
            var ms = new MemoryStream();
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            var xws = new XmlWriterSettings
            {
                Encoding = new UTF8Encoding(false),
                Indent = true
            };

            var xml = new XmlSerializer(typeof(T));
            using (var xw = XmlWriter.Create(ms, xws))
            {
                // 序列化对象
                xml.Serialize(xw, t, ns);
            }

            return xws.Encoding.GetString(ms.ToArray()).Trim();
        }

        /// <summary>
        /// 反序列化为对象
        /// </summary>
        /// <typeparam name="T"> 对象类型 </typeparam>
        /// <param name="s"> 对象序列化后的 </param>
        /// <returns></returns>
        public static T Deserialize<T>(string s)
        {
            var xr = XmlReader.Create(new StringReader(s));
            var xs = new XmlSerializer(typeof(T));
            var obj = (T)xs.Deserialize(xr);

            return obj;
        }

        public static void SaveObjectToFile<T>(T obj, string filePath)
        {
            if (null == obj || null == filePath || filePath.Trim().Length <= 0)
            {
                return;
            }

            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath) ?? throw new InvalidOperationException());
            }

            if (!File.Exists(filePath))
            {
                //File.Create(filePath);
                var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write);
                var buffer = new UTF8Encoding(false).GetBytes(Serialize<T>(obj));
                fs.Write(buffer, 0, buffer.Length);
                fs.Close();
            }
            else
            {
                var fs = new FileStream(filePath, FileMode.Truncate, FileAccess.Write, FileShare.Write);
                var buffer = new UTF8Encoding(false).GetBytes(Serialize<T>(obj));
                fs.Write(buffer, 0, buffer.Length);
                fs.Close();
            }
           
        }

        public static T ReadObjectFromFile<T>(string filePath)
        {
            if (null == filePath || filePath.Trim().Length <= 0 || !File.Exists(filePath))
            {
                return default(T);
            }

            var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
            var buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            var content = GetUtf8String(buffer);
            fs.Close();

            return Deserialize<T>(content);
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected static string GetUtf8String(byte[] buffer)
        {
            if (buffer == null)
                return null;

            if (buffer.Length <= 3)
            {
                return Encoding.UTF8.GetString(buffer);
            }

            byte[] bomBuffer = new byte[] { 0xef, 0xbb, 0xbf };  

            if (buffer[0] == bomBuffer[0]
                && buffer[1] == bomBuffer[1]
                && buffer[2] == bomBuffer[2])
            {
                return new UTF8Encoding(false).GetString(buffer, 3, buffer.Length - 3);
            }

            return Encoding.UTF8.GetString(buffer);
        }
    }
}
