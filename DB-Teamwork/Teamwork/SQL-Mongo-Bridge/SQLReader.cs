using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Supermarket.EF.Data;
using System.IO;
using Newtonsoft.Json;

namespace SQL_Mongo_Bridge
{
    public static class SQLReader
    {
        private static SuperMarketEntities context = null;

        private static List<ProductReport> productReports = null;

        private const string PATH = @"..\..\..\..\WritingFiles\JSON\";

        private const string JSONEXTENSION = ".json";

        static SQLReader()
        {
            SQLReader.productReports = new List<ProductReport>();
        }

        public static void WriteToFileSystem()
        {
            string raportAsString = null;
           
            foreach (var productRaport in SQLReader.productReports)
            {
               
                raportAsString = JsonConvert.SerializeObject(productRaport,Formatting.Indented);
                File.WriteAllText(SQLReader.PATH + productRaport.ProductId.ToString() + SQLReader.JSONEXTENSION ,raportAsString);
            }
        }

        public static List<ProductReport> GetProductReports()
        {
            using (context = new SuperMarketEntities())
            {
                var result = (from product in context.Products
                             join vendor in context.Vendors
                             on product.VendorID equals vendor.VendorID
                             join saleReport in context.SalesReports
                             on product.ProductID equals saleReport.ProductID
                             select new
                             {
                                 ProductId = product.ProductID,
                                 ProductName = product.ProductName,
                                 VendorName = vendor.VendorName,
                                 Income = saleReport.Sum,
                                 Quantity = saleReport.Quantity
                             }).GroupBy(x => x.ProductId);
                            
                foreach (var item in result)
                {
                    ProductReport currReport = new ProductReport();
                    currReport.ProductId = item.First().ProductId;
                    currReport.ProductName = item.First().ProductName;
                    currReport.VendorName = item.First().VendorName;

                    decimal totalIncome = 0;
                    int totalQuantities = 0;

                    foreach (var sp in item)
                    {
                        totalIncome = totalIncome + sp.Income;
                        totalQuantities = totalQuantities + sp.Quantity;

                    }

                    currReport.TotalIncomes = totalIncome;
                    currReport.TotalQuantitySold = totalQuantities;

                    SQLReader.productReports.Add(currReport);
                }
            }

            return SQLReader.productReports;
        }
    }
}
