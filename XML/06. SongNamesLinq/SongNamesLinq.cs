using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

class SongNamesLinq
{
    static void Main()
    {
        Console.WriteLine("Song titles in the catalogue:");
        XDocument catalogue = XDocument.Load("../../catalogue.xml");
        var songs =
          from album in catalogue.Descendants("album")
          from song in album.Descendants("song")
          select song.Element("title").Value;
        foreach (var item in songs)
        {
            Console.WriteLine(item);
        }
    }
}