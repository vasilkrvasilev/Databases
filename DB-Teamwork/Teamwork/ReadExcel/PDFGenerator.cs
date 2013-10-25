using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supermarket.EF.Data;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using Docs.Pdf;

namespace Document_IO_Operations
{
    public static class PDFGenerator
    {
        private const string PDF_PATH = @"..\..\..\..\WritingFiles\report.pdf";

        public static void Generate()
        {
            StringBuilder html = new StringBuilder();
            html.Append("<!DOCTYPE html><html><body><h1>Aggregated Sales Report</h1>");
            SuperMarketEntities dbContext = new SuperMarketEntities();
            using (dbContext)
            {
                var dateList = dbContext.SalesReports.Select(x => x.Date).Distinct();
                foreach (var date in dateList)
                {
                    html.Append(string.Format("<h4 style=\"color:#555\">Date: {0}</h4>",
                        date.ToString("dd-MMM-yyyy", new CultureInfo("en-US"))));


                    var products = dbContext.SalesReports.
                        Join(dbContext.Products,
                        (s => s.ProductID), (p => p.ProductID), (s, p) =>
                            new
                            {
                                Date = s.Date,
                                Product = p.ProductName,
                                Quantity = s.Quantity,
                                MeasureID = p.MeasureID,
                                Price = s.UnitPrice,
                                LocationID = s.LocationID,
                                Sum = s.Sum
                            });
                    var measures = products.Join(dbContext.Measures,
                            (sp => sp.MeasureID), (m => m.MeasureID), (sp, m) =>
                            new
                            {
                                Date = sp.Date,
                                Product = sp.Product,
                                Quantity = sp.Quantity,
                                Measure = m.Name,
                                Price = sp.Price,
                                LocationID = sp.LocationID,
                                Sum = sp.Sum
                            });
                    var locations = measures.Join(dbContext.Locations,
                            (spm => spm.LocationID), (l => l.LocationID), (spm, l) =>
                            new
                            {
                                Date = spm.Date,
                                Product = spm.Product,
                                Quantity = spm.Quantity,
                                Measure = spm.Measure,
                                Price = spm.Price,
                                Location = l.Supermarket,
                                Sum = spm.Sum
                            }).Select(spml => spml).
                            Where(spml => spml.Date == date);

                    html.Append("<div><table border=\"1\"><tr bgcolor=\"#aaa\">" +
                        "<th>Product</th><th>Quantity</th><th>Unit Price</th><th>Location</th><th>Sum</th></tr><tr>");
                    decimal totalSum = 0m;
                    foreach (var item in locations)
                    {
                        html.Append(string.Format("<td>{0}</td><td>{1}</td><td>{2:F2}</td><td>{3}</td><td>{4:F2}</td></tr><tr>",
                            item.Product, item.Quantity + " " + item.Measure, item.Price, 
                            item.Location.Replace('“', '"').Replace('–', '-').Replace('”', '"'), item.Sum));
                        totalSum += item.Sum;
                    }

                    html.Append(string.Format("<td colspan=\"4\">Total sum for {0}:</td><td>{1:F2}</td></tr><tr></table></div>",
                            date.ToString("dd-MMM-yyyy", new CultureInfo("en-US")), totalSum));
                }
            }

            html.Append("</body></html>");
      
            Docs.Pdf.HtmlToPdf convertion = new Docs.Pdf.HtmlToPdf();

            convertion.OpenHTML(html.ToString());
            convertion.SavePDF(PDFGenerator.PDF_PATH);
        }
    }
}
