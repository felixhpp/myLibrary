using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CommonLib.Xml
{
    /// <summary>
    /// XML序列化和反序列化通用类
    /// </summary>
    public class XmlSerializerUtil
    {
        /// <summary>
        /// 序列化对象为Xml字符串
        /// </summary>
        /// <typeparam name="T">要序列化的对象类型</typeparam>
        /// <param name="item">要序列化的对象</param>
        /// <returns>序列化后的xml字符串</returns>
        public static string XmlSerialize<T>(T item)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StringBuilder stringBuilder = new StringBuilder();
            using (StringWriter writer = new StringWriter(stringBuilder))
            {
                serializer.Serialize(writer, item);
            }
            return stringBuilder.ToString();
        }


        /// <summary>
        /// 序列化object为 xml
        /// </summary>
        /// <param name="item">object对象</param>
        /// <returns>序列化后的xml字符串</returns>
        public static string XmlSerialize(object item)
        {
            Type type = item.GetType();
            XmlSerializer serializer = new XmlSerializer(type);
            StringBuilder stringBuilder = new StringBuilder();
            using (StringWriter writer = new StringWriter(stringBuilder))
            {
                serializer.Serialize(writer, item);
            }
            return stringBuilder.ToString();
        }


        /// <summary>
        /// 从xml反序列化到适当的类型对象。
        /// </summary>
        /// <typeparam name="T">反序列化的对象类型</typeparam>
        /// <param name="xmlData">XML字符串</param>
        /// <returns>反序列化后的对象</returns>
        public static T XmlDeserialize<T>(string xmlData)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (TextReader reader = new StringReader(xmlData))
            {
                T entity = (T)serializer.Deserialize(reader);
                return entity;
            }
        }
    }
}
