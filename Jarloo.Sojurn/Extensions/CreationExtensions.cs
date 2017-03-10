using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Jarloo.Sojurn.Extensions
{
    public static class CreationExtensions
    {
        /// <summary>
        /// Creates a Deep Clone of an object that implements ISerializable
        /// </summary>
        /// <typeparam name="T">The Type of the object to clone</typeparam>
        /// <param name="original">The object to clone</param>
        /// <returns>The deep cloned object</returns>
        public static T Clone<T>(this T original) where T : ISerializable
        {
            if (ReferenceEquals(original, null)) return default(T);

            using (Stream stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, original);
                stream.Seek(0, SeekOrigin.Begin);
                return (T) formatter.Deserialize(stream);
            }
        }
    }
}