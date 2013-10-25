using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

class TraverseX
{
    static void Main()
    {
        Console.WriteLine("Enter directory with escaping");
        string path = Console.ReadLine();

        XElement fileSystem = new XElement("file-system");
        TraverseDFS(new DirectoryInfo(path), fileSystem);

        Console.WriteLine(fileSystem);
        fileSystem.Save("../../file-system.xml");
    }

    private static void TraverseDFS(DirectoryInfo directory, XElement parent)
    {
        XElement dir = new XElement("dir");
        dir.Add(new XElement("name", directory.Name));

        FileInfo[] files = directory.GetFiles();
        foreach (var file in files)
        {
            dir.Add(new XElement("file", file.Name));
        }

        DirectoryInfo[] children = directory.GetDirectories();
        foreach (DirectoryInfo child in children)
        {
            TraverseDFS(child, dir);
        }

        parent.Add(dir);
    }
}