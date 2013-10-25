using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

class SongNames
{
    static void Main()
    {
        Console.WriteLine("Song titles in the catalogue:");
        XmlReader reader = XmlReader.Create("../../catalogue.xml");
        using (reader)
        {
            while (reader.Read())
            {
                if ((reader.NodeType == XmlNodeType.Element) &&
                    (reader.Name == "title"))
                {
                    Console.WriteLine(reader.ReadElementString());
                }
            }
        }
    }
}