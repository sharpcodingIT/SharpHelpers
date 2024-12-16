// (c) 2019 SharpCoding
// This code is licensed under MIT license (see LICENSE.txt for details)
using System.IO;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;

namespace SharpCoding.SharpHelpers.ObjectExtensions
{
    public static class ObjectSerializationHelper
    {
        /// <summary>
        /// Serializes the specified object to a JSON string.
        /// Returns an empty string if the object is null or the serialization fails.
        /// </summary>
        /// <param name="instance">The object to serialize.</param>
        /// <returns>A JSON string representation of the object.</returns>
        public static string SerializeToJson(this object instance)
        {
            if (instance == null) return string.Empty;

            try
            {
                return JsonSerializer.Serialize(instance);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Deserializes a JSON string to an object of the specified type.
        /// Returns null if the string is null, empty, or the deserialization fails.
        /// </summary>
        /// <typeparam name="T">The type to deserialize to.</typeparam>
        /// <param name="instance">The JSON string to deserialize.</param>
        /// <returns>An object of the specified type or null.</returns>
        public static T DeserializeFromJson<T>(this string instance) where T : class
        {
            if (string.IsNullOrWhiteSpace(instance)) return null;

            try
            {
                return JsonSerializer.Deserialize<T>(instance);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Serializes the specified object to an XML string.
        /// Returns an empty string if the object is null or the serialization fails.
        /// </summary>
        /// <param name="instance">The object to serialize.</param>
        /// <returns>An XML string representation of the object.</returns>
        public static string SerializeToXml(this object instance)
        {
            if (instance == null) return string.Empty;

            try
            {
                var xmlSerializer = new XmlSerializer(instance.GetType());

                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true }))
                    {
                        xmlSerializer.Serialize(xmlWriter, instance);
                        return stringWriter.ToString();
                    }
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Deserializes an XML string to an object of the specified type.
        /// Returns null if the string is null, empty, or the deserialization fails.
        /// </summary>
        /// <typeparam name="T">The type to deserialize to.</typeparam>
        /// <param name="xmlString">The XML string to deserialize.</param>
        /// <returns>An object of the specified type or null.</returns>
        public static T DeserializeFromXml<T>(this string xmlString) where T : class
        {
            if (string.IsNullOrWhiteSpace(xmlString)) return null;

            try
            {
                var xmlSerializer = new XmlSerializer(typeof(T));

                using (var stringReader = new StringReader(xmlString))
                {
                    return xmlSerializer.Deserialize(stringReader) as T;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
