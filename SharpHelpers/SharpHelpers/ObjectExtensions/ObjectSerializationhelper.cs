// (c) 2019 SharpCoding
// This code is licensed under MIT license (see LICENSE.txt for details)
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text.Json;

namespace SharpCoding.SharpHelpers.ObjectExtensions
{
    public static class ObjectSerializationHelper
    {
        /// <summary>
        /// This method serializes the specific object to JSON
        /// </summary>
        /// <param name="istance"></param>
        /// <returns></returns>
        public static string SerializeToJson(this object istance)
        {
            return istance == null ? string.Empty : JsonSerializer.Serialize(istance);
        }

        /// <summary>
        /// This method deserializes the JSON to the specific object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="istance"></param>
        /// <returns></returns>
        public static T DeserializeFromJson<T>(this string istance) where T : class
        {
            return string.IsNullOrEmpty(istance) ? default : JsonSerializer.Deserialize<T>(istance);
        }

        /// <summary>
        /// This method serializes the specific object to XML
        /// </summary>
        /// <param name="istance"></param>
        /// <returns></returns>
        public static string SerializeToXml(this object istance)
        {
            if (istance == null) return string.Empty;

            var xmlSerializer = new XmlSerializer(istance.GetType());

            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter))
                {
                    xmlSerializer.Serialize(xmlWriter, istance);
                    return stringWriter.ToString();
                }
            }
        }
    }
}
