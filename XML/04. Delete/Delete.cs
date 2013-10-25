using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

class Delete
{
    static void Main()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load("../../catalogue.xml");
        XmlNode rootNode = doc.DocumentElement;
        int count = 0;
        for (int index = 0; index < rootNode.ChildNodes.Count; index++)
        {
            var node = rootNode.ChildNodes.Item(index);
            if (decimal.Parse(node["price"].InnerText) > 10m)
            {
                rootNode.RemoveChild(node);
                count++;
            }
        }

        Console.WriteLine("{0} items deleted", count);
    }
}