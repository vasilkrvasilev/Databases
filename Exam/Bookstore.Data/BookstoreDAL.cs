using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Data
{
    public class BookstoreDAL
    {
        private static readonly string dateFormat = "d-MMM-yyyy";

        public static void AddBook(string author, string title,
            string isbn, string price, string website, BookstoreEntities context)
        {
            Book newBook = new Book();
            newBook.Authors.Add(CreateOrLoadAuthor(author.Trim(), context));
            newBook.Title = title.Trim();
            if (isbn != null)
            {
                newBook.ISBN = isbn.Trim();
            }
            if (price != null)
            {
                newBook.Price = decimal.Parse(price);
            }
            if (website != null)
            {
                newBook.WebSite = website.Trim();
            }

            context.Books.Add(newBook);
            context.SaveChanges();
        }

        public static void AddBookWithReview(List<string> authorNames, string title,
            string isbn, string price, string website, List<Review> reviewList, BookstoreEntities context)
        {
            Book newBook = new Book();
            foreach (var author in authorNames)
            {
                newBook.Authors.Add(CreateOrLoadAuthor(author.Trim(), context));
            }

            newBook.Title = title.Trim();
            if (isbn != null)
            {
                newBook.ISBN = isbn.Trim();
            }
            if (price != null)
            {
                newBook.Price = decimal.Parse(price);
            }
            if (website != null)
            {
                newBook.WebSite = website.Trim();
            }

            foreach (var review in reviewList)
            {
                newBook.Reviews.Add(review);
            }

            context.Books.Add(newBook);
            context.SaveChanges();
        }

        public static Author CreateOrLoadAuthor(string author, BookstoreEntities context)
        {
            Author currentAuthor = context.Authors.FirstOrDefault(x => x.Name.ToLower() == author.ToLower());
            if (currentAuthor == null)
            {
                currentAuthor = new Author();
                currentAuthor.Name = author;
                context.Authors.Add(currentAuthor);
                context.SaveChanges();
            }

            return currentAuthor;
        }


        public static IList<Book> FindBook(string title, string author, string isbn, BookstoreEntities context)
        {
            var bookQuery = context.Books.Select(x => x);
            if (title != null)
            {
                bookQuery = bookQuery.Select(x => x).
                    Where(x => x.Title == title.Trim());
            }

            if (author != null)
            {
                bookQuery = bookQuery.Select(x => x).
                    Where(x => x.Authors.Any(a => a.Name.ToLower() == author.ToLower().Trim()));
            }

            if (isbn != null)
            {
                bookQuery = bookQuery.Select(x => x).
                    Where(x => x.ISBN == isbn.Trim());
            }

            bookQuery = bookQuery.OrderBy(x => x.Title);
            return bookQuery.ToList();
        }

        public static IList<Review> FindReviewPeriod(string start, string end, BookstoreEntities context)
        {
            var reviewQuery = context.Reviews.Include("Author").Include("Book").Include("Book.Authors").Select(x => x);
            if (start != null && end != null)
            {
                DateTime startDate = DateTime.ParseExact(start, dateFormat, new CultureInfo("en-US"));
                DateTime endDate = DateTime.ParseExact(end, dateFormat, new CultureInfo("en-US"));
                reviewQuery = reviewQuery.Select(x => x).
                    Where(x => x.Date >= startDate && x.Date <= endDate);
            }

            reviewQuery = reviewQuery.OrderBy(x => x.Date).OrderBy(x => x.ReviewText);
            return reviewQuery.ToList();
        }

        public static IList<Review> FindReviewAuthor(string author, BookstoreEntities context)
        {
            var reviewQuery = context.Reviews.Include("Author").Include("Book").Include("Book.Authors").Select(x => x);
            if (author != null)
            {
                reviewQuery = reviewQuery.Select(x => x).
                    Where(x => x.Author.Name.ToLower() == author.ToLower().Trim());
            }

            reviewQuery = reviewQuery.OrderBy(x => x.Date).OrderBy(x => x.ReviewText);
            return reviewQuery.ToList();
        }
    }
}