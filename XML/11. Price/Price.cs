using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

class Price
{
    static void Main()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load("../../catalogue.xml");
        XmlNode rootNode = doc.DocumentElement;
        int startYear = DateTime.Now.Year + 5;
        var albums = rootNode.SelectNodes(string.Format("/catalogue/album[year>'{0}']", startYear));
        for (int index = 0; index < albums.Count; index++)
        {
            var name = albums.Item(index).ChildNodes.Item(0).InnerText;
            var price = albums.Item(index).ChildNodes.Item(4).InnerText;
            Console.WriteLine("Album: {0}, Price: {1}", name, price);
        }
    }
}