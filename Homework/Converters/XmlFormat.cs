using Newtonsoft.Json;
using System.Xml;

namespace Homework.Converters
{
    public class XmlFormat : IFormatBase
    {
        /// <summary>
        /// XML to JSON
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public string Deserialize(string content)
        {
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(content);
                return JsonConvert.SerializeXmlNode(doc);
            }
            catch (XmlException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        /// <summary>
        /// JSON to XML
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        /// <exception cref="InvalidDataException"></exception>
        public string Serialize(string content)
        {
            var doc = JsonConvert.DeserializeXNode(content, "root") ?? throw new InvalidDataException("Neplatna data");

            return doc.ToString();
        }
    }
}
