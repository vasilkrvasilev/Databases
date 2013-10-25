using Bookstore.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;

public class DataImporter
{
    private static readonly string anonymous = "anonymous";
    private static readonly string dateFormat = "d-MMM-yyyy";

    static void Main()
    {
        SimpleImport();
        ComplexImport();
    }

    static void SimpleImport()
    {
        BookstoreEntities context = new BookstoreEntities();
        using (context)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Bookstore.Data.Settings.Default.simpleImportFile);
            XmlNodeList catalog = doc.SelectNodes("/catalog/book");
            foreach (XmlNode book in catalog)
            {
                string author = GetValue(book, "author");
                string title = GetValue(book, "title");
                string isbn = GetValue(book, "isbn");
                string price = GetValue(book, "price");
                string website = GetValue(book, "web-site");

                BookstoreDAL.AddBook(author, title, isbn, price, website, context);
            }
        }
    }

    static void ComplexImport()
    {
        BookstoreEntities context = new BookstoreEntities();
        using (context)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Bookstore.Data.Settings.Default.complexImportFile);
            XmlNodeList catalog = doc.SelectNodes("/catalog/book");
            foreach (XmlNode book in catalog)
            {
                TransactionScope scope = new TransactionScope(
                    TransactionScopeOption.Required,
                        new TransactionOptions()
                        {
                            IsolationLevel = IsolationLevel.RepeatableRead
                        });
                using (scope)
                {
                    List<string> authorNames = new List<string>();
                    XmlNodeList authors = book.SelectNodes("authors/author");
                    foreach (XmlNode author in authors)
                    {
                        authorNames.Add(author.InnerText);
                    }

                    string title = GetValue(book, "title");
                    string isbn = GetValue(book, "isbn");
                    string price = GetValue(book, "price");
                    string website = GetValue(book, "web-site");

                    List<Review> reviewList = new List<Review>();
                    XmlNodeList reviews = book.SelectNodes("reviews/review");
                    foreach (XmlNode review in reviews)
                    {
                        Review currentReview = new Review();
                        currentReview.ReviewText = review.InnerText;
                        currentReview.Date = GetDate(review);
                        string authorName = GetAuthor(review);
                        currentReview.Author = BookstoreDAL.CreateOrLoadAuthor(authorName, context);
                        reviewList.Add(currentReview);
                    }

                    BookstoreDAL.AddBookWithReview(authorNames, title, isbn, price, website, reviewList, context);
                    scope.Complete();
                }
            }
        }
    }

    public static string GetValue(XmlNode node, string element)
    {
        XmlNode child = node.SelectSingleNode(element);
        if (child == null)
        {
            return null;
        }

        return child.InnerText;
    }

    public static DateTime GetDate(XmlNode node)
    {
        DateTime date = DateTime.Now;
        var attributeDate = node.Attributes["date"];
        if (attributeDate != null)
        {
            date = DateTime.ParseExact(attributeDate.Value, dateFormat, new CultureInfo("en-US"));
        }

        return date;
    }

    public static string GetAuthor(XmlNode node)
    {
        string author = anonymous;
        var attributeAuthor = node.Attributes["author"];
        if (attributeAuthor != null)
        {
            author = attributeAuthor.Value;
        }

        return author;
    }
}