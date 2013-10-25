using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

class CreateXml
{
    static void Main()
    {
        StreamReader reader = new StreamReader("../../list.txt", Encoding.GetEncoding("utf-8"));
        XElement list = new XElement("list");
        using (reader)
        {
            string line = reader.ReadLine();
            while (line != null)
            {
                string[] data = line.Split(',');
                XElement person = new XElement("person",
                    new XElement("name", data[0]),
                    new XElement("address", data[1]),
                    new XElement("phone", data[2])
                    );
                list.Add(person);
                line = reader.ReadLine();
            }
        }

        System.Console.WriteLine(list);

        list.Save("../../list.xml");
    }
}