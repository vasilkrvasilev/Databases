using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

class XPath
{
    static void Main()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load("../../catalogue.xml");
        XmlNode rootNode = doc.DocumentElement;
        var artists = rootNode.SelectNodes("/catalogue/album/artist");
        for (int index = 0; index < artists.Count; index++)
        {
            var name = artists.Item(index).InnerText;
            var albums = rootNode.SelectNodes(string.Format("/catalogue/album[artist='{0}']", name));
            Console.WriteLine("Artist: {0}, Number of albums: {1}", name, albums.Count);
        }
    }
}