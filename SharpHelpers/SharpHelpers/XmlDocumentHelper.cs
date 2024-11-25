// (c) 2019 SharpCoding
// This code is licensed under MIT license (see LICENSE.txt for details)
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace SharpCoding.SharpHelpers
{
    public static class XmlDocumentHelper
    {
        /// <summary>
        /// Converts an XDocument to an XmlDocument.
        /// </summary>
        /// <param name="xDocument">The XDocument to convert.</param>
        /// <returns>The converted XmlDocument.</returns>
        /// <exception cref="ArgumentNullException">Thrown when xDocument is null.</exception>
        public static XmlDocument ToXmlDocument(this XDocument xDocument)
        {
            if (xDocument == null) throw new ArgumentNullException(nameof(xDocument));

            var xmlDocument = new XmlDocument();
            using (var xmlReader = xDocument.CreateReader())
            {
                xmlDocument.Load(xmlReader);
            }
            return xmlDocument;
        }

        /// <summary>
        /// Converts an XmlDocument to an XDocument.
        /// </summary>
        /// <param name="xmlDocument">The XmlDocument to convert.</param>
        /// <returns>The converted XDocument.</returns>
        /// <exception cref="ArgumentNullException">Thrown when xmlDocument is null.</exception>
        public static XDocument ToXDocument(this XmlDocument xmlDocument)
        {
            if (xmlDocument == null) throw new ArgumentNullException(nameof(xmlDocument));

            using (var nodeReader = new XmlNodeReader(xmlDocument))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
            }
        }

        /// <summary>
        /// Converts an XElement to an XmlDocument.
        /// </summary>
        /// <param name="xElement">The XElement to convert.</param>
        /// <returns>The converted XmlDocument.</returns>
        /// <exception cref="ArgumentNullException">Thrown when xElement is null.</exception>
        public static XmlDocument ToXmlDocument(this XElement xElement)
        {
            if (xElement == null) throw new ArgumentNullException(nameof(xElement));

            var sb = new StringBuilder();
            var settings = new XmlWriterSettings { OmitXmlDeclaration = true, Indent = false };
            using (var xw = XmlWriter.Create(sb, settings))
            {
                xElement.WriteTo(xw);
            }
            var doc = new XmlDocument();
            doc.LoadXml(sb.ToString());
            return doc;
        }

        /// <summary>
        /// Finds the first child node with the specified name.
        /// </summary>
        /// <param name="xmlDocument">The XmlDocument to search.</param>
        /// <param name="nodeName">The name of the node to find.</param>
        /// <returns>The first matching XmlNode, or null if not found.</returns>
        public static XmlNode FindFirstChild(this XmlDocument xmlDocument, string nodeName)
        {
            if (xmlDocument == null) throw new ArgumentNullException(nameof(xmlDocument));
            if (string.IsNullOrWhiteSpace(nodeName)) throw new ArgumentException("Node name cannot be null or empty.", nameof(nodeName));

            return xmlDocument.SelectSingleNode($"//{nodeName}");
        }

        /// <summary>
        /// Adds a new child element with the specified name and value to the given parent node.
        /// </summary>
        /// <param name="xmlDocument">The XmlDocument to modify.</param>
        /// <param name="parentNode">The parent node to which the new element will be added.</param>
        /// <param name="elementName">The name of the new element.</param>
        /// <param name="elementValue">The value of the new element.</param>
        /// <returns>The newly created XmlElement.</returns>
        public static XmlElement AddChildElement(this XmlDocument xmlDocument, XmlNode parentNode, string elementName, string elementValue)
        {
            if (xmlDocument == null) throw new ArgumentNullException(nameof(xmlDocument));
            if (parentNode == null) throw new ArgumentNullException(nameof(parentNode));
            if (string.IsNullOrWhiteSpace(elementName)) throw new ArgumentException("Element name cannot be null or empty.", nameof(elementName));

            var newElement = xmlDocument.CreateElement(elementName);
            newElement.InnerText = elementValue;
            parentNode.AppendChild(newElement);
            return newElement;
        }

        /// <summary>
        /// Converts the XmlDocument to a formatted string.
        /// </summary>
        /// <param name="xmlDocument">The XmlDocument to convert.</param>
        /// <returns>A string representation of the XmlDocument.</returns>
        public static string ToFormattedString(this XmlDocument xmlDocument)
        {
            if (xmlDocument == null) throw new ArgumentNullException(nameof(xmlDocument));

            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true }))
                {
                    xmlDocument.Save(xmlWriter);
                }
                return stringWriter.ToString();
            }
        }

        /// <summary>
        /// Removes the specified node from the XmlDocument.
        /// </summary>
        /// <param name="xmlDocument">The XmlDocument to modify.</param>
        /// <param name="node">The node to remove.</param>
        public static void RemoveNode(this XmlDocument xmlDocument, XmlNode node)
        {
            if (xmlDocument == null) throw new ArgumentNullException(nameof(xmlDocument));
            if (node == null) throw new ArgumentNullException(nameof(node));

            if (node.ParentNode != null)
            {
                node.ParentNode.RemoveChild(node);
            }
        }
    }
}
