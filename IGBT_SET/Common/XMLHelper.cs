using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IGBT_SET.Common
{
    public class XMLHelper
    {
        public static string SerializeObject<T>(T obj)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, obj);
                return textWriter.ToString();
            }
        }

        public static T DeserializeObject<T>(string xml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            using (StringReader textReader = new StringReader(xml))
            {
                return (T)xmlSerializer.Deserialize(textReader);
            }
        }

        public static string SerializeList<T>(List<T> list)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<T>));

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, list);
                return textWriter.ToString();
            }
        }

        public static List<T> DeserializeList<T>(string xml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<T>));

            using (StringReader textReader = new StringReader(xml))
            {
                return (List<T>)xmlSerializer.Deserialize(textReader);
            }
        }

        public static void SaveXmlToFile<T>(T obj, string filePath)
        {
            string xml = SerializeObject(obj);
            File.WriteAllText(filePath, xml, Encoding.UTF8);
        }

        public static T LoadXmlFromFile<T>(string filePath)
        {
            string xml = File.ReadAllText(filePath, Encoding.UTF8);
            return DeserializeObject<T>(xml);
        }
    }
}