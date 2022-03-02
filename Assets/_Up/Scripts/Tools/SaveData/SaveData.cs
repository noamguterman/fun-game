using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Assets._Scripts.Tools.SaveData
{
    public static class SaveData<T> where T : class
    {
        public static T Load()
        {
            var type = typeof (T);
            var path = string.Format("{0}/{1}.xml", Application.persistentDataPath, type.Name);
            if (!File.Exists(path)) return default(T);
            using (var stream = new FileStream(path, FileMode.Open))
            {
                var serializer = new XmlSerializer(type);
                return serializer.Deserialize(stream) as T;
            }
        }

        public static void Save(T data)
        {
            var type = typeof (T);
            var path = string.Format("{0}/{1}.xml", Application.persistentDataPath, type.Name);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                var serializer = new XmlSerializer(type);
                serializer.Serialize(stream, data);
            }
        }

        public static void Delete()
        {
            var type = typeof(T);
            var path = string.Format("{0}/{1}.xml", Application.persistentDataPath, type.Name);
            File.Delete(path);
        }


        public static void WriteToFile(string data, string path)
        {
            using (var stream = new FileStream(path, FileMode.Create))
            {
                var sw = new StreamWriter(stream);
                sw.WriteLine(data);
                sw.Flush();
            }
        }
        public static string ReadFromFile(string path)
        {
            return File.ReadAllText(path);
        }
    }
}
