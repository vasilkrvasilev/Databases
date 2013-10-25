using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

class PriceLinq
{
    static void Main()
    {
        XDocument catalogue = XDocument.Load("../../catalogue.xml");
        int startYear = DateTime.Now.Year + 5;
        var albums =
          from album in catalogue.Descendants("album")
          where int.Parse(album.Element("year").Value) > startYear
          select new {
              Title = album.Element("name").Value,
              Price = album.Element("price").Value 
          };

        foreach (var item in albums)
        {
            Console.WriteLine("Album: {0}, Price: {1}", item.Title, item.Price);
        }
    }
}