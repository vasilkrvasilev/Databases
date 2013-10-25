using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace SQL_Mongo_Bridge
{
    public static class MongoWriter
    {
        public static void WriteToDatabase()
        {
            MongoClient client = new MongoClient(MongoSettings.Default.MongoConnectionString);
            MongoServer server = client.GetServer();
            MongoDatabase productReportsDb = server.GetDatabase("productReport");

            MongoCollection<ProductReportEntity> reportsCollection = productReportsDb.GetCollection<ProductReportEntity>("reports");
            List<ProductReport> reports = SQLReader.GetProductReports();

            foreach (var report in reports)
            {
                ProductReportEntity reportEntity = new ProductReportEntity()
                {
                    ProductId = report.ProductId,
                    ProductName = report.ProductName,
                    VendorName = report.VendorName,
                    TotalIncomes = report.TotalIncomes,
                    TotalQuantitySold = report.TotalQuantitySold
                };

                reportsCollection.Save(reportEntity);
            }
        }
        
    }
}
