using System;
using System.IO;
using System.Xml.Serialization;

namespace AnimalGuess.Module
{
    public interface IDataRepository<T> where T: class
    {
        void SaveData(T data);
        T GetData();
    }

    /// <summary>
    /// Repository Implementation using XML file.
    /// Author: Alvin Husin
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataRepositoryXml<T> : IDataRepository<T> where T : class
    {
        private readonly string _xmlFile;

        public DataRepositoryXml(string xmlFile)
        {
            if (string.IsNullOrWhiteSpace(xmlFile))
                throw new ArgumentNullException(nameof(xmlFile));

            _xmlFile = xmlFile;
        }

        public void SaveData(T data)
        {
            var xs = new XmlSerializer(typeof(T));
            using (var sw = new StreamWriter(_xmlFile))
            {
                xs.Serialize(sw, data);
            }
        }

        public T GetData()
        {
            if (!File.Exists(_xmlFile))
                throw new FileNotFoundException(_xmlFile);

            var xs = new XmlSerializer(typeof(T));
            using (var sr = new StreamReader(_xmlFile))
            {
                return xs.Deserialize(sr) as T;
            }
        }
    }
}
