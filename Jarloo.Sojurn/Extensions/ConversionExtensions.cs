using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Serialization;

namespace Jarloo.Sojurn.Extensions
{
    public static class ConversionExtensions
    {
        /// <summary>
        /// Serializes an object to JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson<T>(this T obj)
        {
            using (var stream = new MemoryStream())
            {
                var jsSerializer = new DataContractJsonSerializer(typeof (T));
                jsSerializer.WriteObject(stream, obj);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        /// <summary>
        /// Deseralizes an object from JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T FromJson<T>(this string input)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(input)))
            {
                var jsSerializer = new DataContractJsonSerializer(typeof (T));
                var obj = (T) jsSerializer.ReadObject(stream);
                return obj;
            }
        }

        /// <summary>
        /// Serializes and object to XML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToXml<T>(this T obj)
        {
            using (var stream = new MemoryStream())
            {
                var x = new XmlSerializer(typeof (T));
                x.Serialize(stream, obj);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        /// <summary>
        /// Deseralizes an object from XML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T FromXml<T>(this string input)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(input)))
            {
                var x = new XmlSerializer(typeof (T));
                var obj = (T) x.Deserialize(stream);
                return obj;
            }
        }

        /// <summary>
        /// Converts an object to the type specified. 
        /// If the object is a string and null or blank it will return null
        /// If the object is = DBNull it will return the default type for that object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="text"></param>
        /// <returns></returns>
        public static T To<T>(this object text)
        {
            if (text == null) return default(T);
            if (text.Equals(DBNull.Value)) return default(T);
            if (text is string) if (string.IsNullOrWhiteSpace(text as string)) return default(T);

            var type = typeof (T);

            var underlyingType = Nullable.GetUnderlyingType(type) ?? type;
            return (T) Convert.ChangeType(text, underlyingType);
        }

        /// <summary>
        /// Checks if the object is null
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsNull(this object o)
        {
            return o == null;
        }
    }
}