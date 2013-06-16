using System;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Jarloo.Sojurn.Data
{
    public class LocalJsonPersistenceManager : IPersistenceManager
    {
        public void Save<T>(string key, T o)
        {
            string json = ToJson(o);
            string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["DATA_LOCATION"]);
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            string filename = Path.Combine(folder, key + ".json");

            File.WriteAllText(filename, json);
        }

        public T Retrieve<T>(string key) where T : new()
        {
            string filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["DATA_LOCATION"], key + ".json");
            if (!File.Exists(filename)) return default(T);

            string json = File.ReadAllText(filename);
            return FromJson<T>(json);
        }

        private static string ToJson<T>(T obj)
        {
            MemoryStream stream = new MemoryStream();

            try
            {
                DataContractJsonSerializer jsSerializer = new DataContractJsonSerializer(typeof (T));
                jsSerializer.WriteObject(stream, obj);

                return Encoding.UTF8.GetString(stream.ToArray());
            }
            finally
            {
                stream.Close();
                stream.Dispose();
            }
        }

        private static T FromJson<T>(string input)
        {
            MemoryStream stream = new MemoryStream();

            try
            {
                DataContractJsonSerializer jsSerializer = new DataContractJsonSerializer(typeof (T));
                stream = new MemoryStream(Encoding.UTF8.GetBytes(input));
                T obj = (T) jsSerializer.ReadObject(stream);

                return obj;
            }
            finally
            {
                stream.Close();
                stream.Dispose();
            }
        }
    }
}