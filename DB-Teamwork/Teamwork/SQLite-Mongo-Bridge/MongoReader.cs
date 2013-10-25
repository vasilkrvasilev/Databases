using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace SQLite_Mongo_Bridge
{
    public static class MongoReader
    {
        public static List<ProductReportEntity> GetProductReports()
        {
            List<ProductReportEntity> result = new List<ProductReportEntity>();

            string mongoConnectionString = @"mongodb://192.168.193.25:27017/productReport";
            MongoClient client = new MongoClient(mongoConnectionString);
            MongoServer server = client.GetServer();
            MongoDatabase productReportsDb = server.GetDatabase("productReport");

            MongoCollection<ProductReportEntity> reportsCollection = productReportsDb.GetCollection<ProductReportEntity>("reports");
            var reports =
                from report in reportsCollection.AsQueryable<ProductReportEntity>()
                select report;

            foreach (var report in reports)
            {
                ProductReportEntity newReport = new ProductReportEntity();
                newReport.ProductName = report.ProductName;
                newReport.VendorName = report.VendorName;
                newReport.TotalIncomes = report.TotalIncomes;
                newReport.TotalQuantitySold = report.TotalQuantitySold;

                result.Add(newReport);
            }

            return result;
        }
    }
}
