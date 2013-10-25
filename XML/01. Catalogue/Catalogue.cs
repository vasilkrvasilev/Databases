using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

class Catalogue
{
    static void Main()
    {
        XElement catalogueXml = new XElement("catalogue",
            new XElement("album",
                new XElement("name", "Album"),
                new XElement("artist", "Singer"),
                new XElement("year", "2012"),
                new XElement("producer", "P"),
                new XElement("price", "10.39"),
                new XElement("song",
                    new XElement("title", "Song"),
                    new XElement("doration", "4.03")
                    ),
                new XElement("song",
                    new XElement("title", "Not Song"),
                    new XElement("doration", "2.15")
                    )
            ),
            new XElement("album",
                new XElement("name", "Not Album"),
                new XElement("artist", "Not Singer"),
                new XElement("year", "2020"),
                new XElement("producer", "NP"),
                new XElement("price", "0.00"),
                new XElement("song",
                    new XElement("title", "Something"),
                    new XElement("doration", "45.03")
                    )
            )
        );

        Console.WriteLine(catalogueXml);

        catalogueXml.Save("../../catalogue.xml");
    }
}