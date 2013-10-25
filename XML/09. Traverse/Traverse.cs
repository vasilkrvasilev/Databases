using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

class Traverse
{
    static void Main()
    {
        Console.WriteLine("Enter directory with escaping");
        string path = Console.ReadLine();
        StreamWriter textWriter = new StreamWriter("../../file-system.xml", false, Encoding.GetEncoding("utf-8"));
        XmlTextWriter writer = new XmlTextWriter(textWriter);
        using (writer)
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("file-system");
            TraverseDFS(new DirectoryInfo(path), writer);
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }
    }

    private static void TraverseDFS(DirectoryInfo directory, XmlTextWriter writer)
    {    
        writer.WriteStartElement("dir");
        writer.WriteElementString("name", directory.Name);

        FileInfo[] files = directory.GetFiles();
        foreach (var file in files)
        {
            writer.WriteElementString("file", file.Name);
        }

        DirectoryInfo[] children = directory.GetDirectories();
        foreach (DirectoryInfo child in children)
        {
            TraverseDFS(child, writer);
        }

        writer.WriteEndElement();
    }
}
