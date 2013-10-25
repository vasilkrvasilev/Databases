using Bookstore.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

class DataSearcher
{
    private static readonly string dateFormat = "d-MMM-yyyy";

    static void Main()
    {
        SimpleSearch();

        string fileName = Bookstore.Data.Settings.Default.complexSearchWriteFile;
        using (XmlTextWriter writer =
            new XmlTextWriter(fileName, Encoding.UTF8))
        {
            writer.Formatting = Formatting.Indented;
            writer.IndentChar = '\t';
            writer.Indentation = 1;

            writer.WriteStartDocument();
            writer.WriteStartElement("search-results");

            ComplexSearch(writer);

            writer.WriteEndDocument();
        }
    }

    static void SimpleSearch()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(Bookstore.Data.Settings.Default.simpleSearchFile);
        XmlNode query = doc.SelectSingleNode("/query");
        string title = DataImporter.GetValue(query, "title");
        string author = DataImporter.GetValue(query, "author");
        string isbn = DataImporter.GetValue(query, "isbn");
        BookstoreEntities context = new BookstoreEntities();
        using (context)
        {
            IList<Book> found = BookstoreDAL.FindBook(title, author, isbn, context);
            if (found.Count > 0)
            {
                Console.WriteLine("{0} books found:", found.Count);
                foreach (var book in found)
                {
                    if (book.Reviews.Count > 0)
                    {
                        Console.WriteLine("{0} --> {1} reviews", book.Title, book.Reviews.Count);
                    }
                    else
                    {
                        Console.WriteLine("{0} --> no reviews", book.Title);
                    }
                }
            }
            else
            {
                Console.WriteLine("Nothing found");
            }
        }
    }

    static void ComplexSearch(XmlTextWriter writer)
    {
        BookstoreEntities context = new BookstoreEntities();
        using (context)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Bookstore.Data.Settings.Default.complexSearchReadFile);
            XmlNodeList queries = doc.SelectNodes("/review-queries/query");
            foreach (XmlNode query in queries)
            {
                string type = query.Attributes["type"].Value;
                if (type == "by-period")
                {
                    string start = query.SelectSingleNode("start-date").InnerText;
                    string end = query.SelectSingleNode("end-date").InnerText;
                    IList<Review> found =
                        BookstoreDAL.FindReviewPeriod(start, end, context);
                    WriteResultSet(writer, found);
                }
                else if (type == "by-author")
                {
                    string author = query.SelectSingleNode("author-name").InnerText;
                    IList<Review> found =
                        BookstoreDAL.FindReviewAuthor(author, context);
                    WriteResultSet(writer, found);
                }

                SearchLogsImporter.AddLog(DateTime.Now, query.OuterXml);
            }
        }
    }

    private static void WriteResultSet(
        XmlTextWriter writer, IList<Review> found)
    {
        writer.WriteStartElement("result-set");
        foreach (var review in found)
        {
            writer.WriteStartElement("review");
            if (review.Date != null)
            {
                DateTime formatedDate = new DateTime(review.Date.Value.Year, review.Date.Value.Month, review.Date.Value.Day);
                writer.WriteElementString("date", formatedDate.ToString(dateFormat, new CultureInfo("en-US")));
            }
            if (review.ReviewText != null)
            {
                writer.WriteElementString("content", review.ReviewText);
            }
            if (review.Book != null)
            {
                writer.WriteStartElement("book");
                if (review.Book.Title != null)
                {
                    writer.WriteElementString("title", review.Book.Title);
                }
                if (review.Book.Authors.Count > 0)
                {
                    string authors = string.Join(", ",
                        review.Book.Authors.Select(a => a.Name).OrderBy(a => a));
                    writer.WriteElementString("authors", authors);
                }
                if (review.Book.ISBN != null)
                {
                    writer.WriteElementString("isbn", review.Book.ISBN);
                }
                if (review.Book.WebSite != null)
                {
                    writer.WriteElementString("url", review.Book.WebSite);
                }

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        writer.WriteEndElement();
    }
}