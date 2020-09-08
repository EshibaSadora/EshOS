using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Eshiba
{
   class EshXml
    {
        XmlDocument doc = new XmlDocument();
        XmlElement root = null;
        public EshXml(String str)
        {
            doc.LoadXml(str);
            root = doc.DocumentElement;
        }

        public String GetXmlElementByPath(String path)
        {
            return GetXmlElementByPath(root, path, "");
        }

        public String GetXmlElementByPath(String path, String BaseValue)
        {
            return GetXmlElementByPath(root, path, BaseValue);
        }

        public static String GetXmlElementByPath(XmlElement rootElement, String path, String BaseValue)
        {
            String output;
            XmlNode titleNode = rootElement.SelectSingleNode(path);
            if (titleNode == null)
            {
                output = BaseValue;
            }
            else
            {
                output = titleNode.InnerText;
                if (output == null) output = BaseValue;
            }

            return output;
        }

        public static String GetXmlElementByPath(XmlElement rootElement, String path)
        {
            return GetXmlElementByPath(rootElement, path, "");
        }
    }
}
