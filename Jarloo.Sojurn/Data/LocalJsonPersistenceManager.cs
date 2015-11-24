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
            var json = ToJson(o);
            var folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                ConfigurationManager.AppSettings["DATA_LOCATION"]);
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            var filename = Path.Combine(folder, key + ".json");

            //Backup the file in case something happens
            if (File.Exists(filename))
            {
                var backup = Path.Combine(folder, DateTime.Today.ToString("yyyyMMdd_") + key + ".json");
                File.Copy(filename, backup, true);
            }

            File.WriteAllText(filename, json);
        }

        public T Retrieve<T>(string key) where T : new()
        {
            var filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                ConfigurationManager.AppSettings["DATA_LOCATION"], key + ".json");
            if (!File.Exists(filename)) return default(T);

            var json = File.ReadAllText(filename);
            return FromJson<T>(json);
        }

        private static string ToJson<T>(T obj)
        {
            using (var stream = new MemoryStream())
            {
                var jsSerializer = new DataContractJsonSerializer(typeof (T));
                jsSerializer.WriteObject(stream, obj);

                return Encoding.UTF8.GetString(stream.ToArray());             
            }
        }

        private static T FromJson<T>(string input)
        {
            var jsSerializer = new DataContractJsonSerializer(typeof (T));
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(input)))
            {
                var obj = (T) jsSerializer.ReadObject(stream);
                return obj;
            }
        }
    }
}