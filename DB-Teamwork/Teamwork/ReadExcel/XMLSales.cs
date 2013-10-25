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
using System.Xml.Linq;

namespace Document_IO_Operations
{
    public class XMLSales
    {
        public static void Generate()
        {
            SuperMarketEntities dbContext = new SuperMarketEntities();
            XElement xml = new XElement("sales");

            using (dbContext)
            {
                foreach (var vendor in dbContext.Vendors)
                {
                    XElement sales = new XElement("sale");
                    XAttribute currentVendor = new XAttribute("vendor", vendor.VendorName);
                    sales.Add(currentVendor);

                    var query = from s in dbContext.SalesReports
                                join p in dbContext.Products on s.ProductID equals p.ProductID
                                join v in dbContext.Vendors on p.VendorID equals v.VendorID
                                where v.VendorName == vendor.VendorName
                                group s by s.Date into d
                                select new
                                {
                                    date = d.Key,
                                    totalSum = d.Sum(x => x.Sum)
                                };

                    foreach (var summary in query)
                    {
                        XElement currentSummary = new XElement("summary");

                        string formatDate = summary.date.ToString("dd-MMM-yyyy", new CultureInfo("en-US"));

                        XAttribute currentDate = new XAttribute("date", formatDate);
                        currentSummary.Add(currentDate);

                        string formatTotalSum = summary.totalSum.ToString("F2", new CultureInfo("en-US"));

                        XAttribute currentSum = new XAttribute("total-sum", formatTotalSum);
                        currentSummary.Add(currentSum);

                        sales.Add(currentSummary);
                    }

                    xml.Add(sales);
                }

                xml.Save("../../../../WritingFiles/Sales-by-Vendors-report.xml");
            }
        }
    }
}