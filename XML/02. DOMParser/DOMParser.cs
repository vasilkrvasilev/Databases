using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

class DOMParser
{
    static void Main()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load("../../catalogue.xml");
        XmlNode rootNode = doc.DocumentElement;
        List<string> artists = new List<string>();
        foreach (XmlNode album in rootNode.ChildNodes)
        {
            var name = album.ChildNodes[1].InnerText;
            if (!artists.Contains(name))
            {
                artists.Add(name);
            }
        }

        foreach (var item in artists)
        {
            int count = 0;
            foreach (XmlNode node in rootNode.ChildNodes)
            {
                if (node["artist"].InnerText == item)
                {
                    count++;
                }
            }

            Console.WriteLine("Artist: {0}, Number of albums: {1}", item, count);
        }
    }
}