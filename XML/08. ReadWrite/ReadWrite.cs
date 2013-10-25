using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

class ReadWrite
{
    static void Main()
    {
        XmlReader reader = XmlReader.Create("../../catalogue.xml");
        using (reader)
        {
            StreamWriter textWriter = new StreamWriter("../../albums.xml", false, Encoding.GetEncoding("utf-8"));
            XmlTextWriter writer = new XmlTextWriter(textWriter);
            using (writer)
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("album-catalog");
                while (reader.Read())
                {
                    if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "album"))
                    {
                        writer.WriteStartElement("album");
                        reader.ReadToDescendant("name");
                        writer.WriteElementString(reader.Name, reader.ReadInnerXml());
                        reader.ReadToNextSibling("artist");
                        writer.WriteElementString(reader.Name, reader.ReadInnerXml());
                        writer.WriteEndElement();
                    }
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
    }
}