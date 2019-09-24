using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace LabsterHub
{
    public static class XmlUtils
    {

        public static List<XElement> Elements(XElement document, string elementName)
        {
            var descendants = document.Descendants().ToList();
            var elementsWithName = descendants.Where(e => e.Name.LocalName == elementName).ToList();
            return elementsWithName;
        }

        public static List<XElement> Elements(XDocument document, string elementName)
        {
            var descendants = document.Descendants().ToList();
            var elementsWithName = descendants.Where(e => e.Name.LocalName == elementName).ToList();
            return elementsWithName;
        }

        public static bool HasAttribute(this XElement element, string name)
        {
            var attribute = element.Attribute(XName.Get(name));
            return attribute != null;
        }


        public static XAttribute GetAttribute(this XElement element, string attributeName)
        {
            if (HasAttribute(element, attributeName) == false)
            {
                throw new System.Exception("Error finding attribute " + attributeName + " for element");
            }
            return element.Attribute(XName.Get(attributeName));
        }

        public static string GetId(this XElement element)
        {
            var attribute = element.Attribute(XName.Get("Id"));
            return attribute == null ? null : attribute.Value;
        }


    }
}
