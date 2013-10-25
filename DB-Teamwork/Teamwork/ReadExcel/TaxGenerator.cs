using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supermarket.EF.Data;
using System.Data.SQLite;
using System.Data.OleDb;
using System.Reflection;
using Microsoft.Office.Interop.Excel;

namespace Document_IO_Operations
{
    public static class TaxGenerator
    {
        public static Dictionary<string, List<Tuple<string, decimal>>> GetVendorsAndProducts(DateTime checkDate)
        {
            Dictionary<string, List<Tuple<string, decimal>>> vendorAndProducts = new Dictionary<string, List<Tuple<string, decimal>>>();

            using (var context = new SuperMarketEntities())
            {
                var vendors =
                    (from vendor in context.Vendors
                     join product in context.Products
                     on vendor.VendorID equals product.VendorID
                     select new
                     {
                         VendorID = vendor.VendorID,
                         ProductID = product.ProductID,
                         VendorName = vendor.VendorName,
                         ProductName = product.ProductName,
                     }).GroupBy(x => x.VendorName);


                foreach (var vendor in vendors)
                {
                    List<Tuple<string, decimal>> currVendorProducts = new List<Tuple<string, decimal>>();

                    foreach (var product in vendor)
                    {
                        var query = from s in context.SalesReports
                                    where s.ProductID == product.ProductID && s.Date.Month == checkDate.Month && s.Date.Year == checkDate.Year
                                    group s by new { s.Date.Month, s.Date.Year } into d
                                    select new
                                    {
                                        date = d.Key,
                                        totalSum = d.Sum(x => x.Sum)
                                    };
                        currVendorProducts.Add(new Tuple<string, decimal>(product.ProductName, query.First().totalSum));
                    }

                    vendorAndProducts.Add(vendor.First().VendorName, currVendorProducts);
                }
            }

            return vendorAndProducts;
        }

        public static Dictionary<string, decimal> GetProductTaxes()
        {
            Dictionary<string, decimal> productTaxes = new Dictionary<string, decimal>();

            const string ConnectionString = "Data Source=..\\..\\..\\..\\SQLiteDatabase\\SupermarketFinance.db";
            SQLiteConnection sqliteConnection = new SQLiteConnection(ConnectionString);
            sqliteConnection.Open();
            using (sqliteConnection)
            {
                string commandText = "SELECT ProductName, Tax FROM ProductTaxes";
                SQLiteCommand command = new SQLiteCommand(commandText, sqliteConnection);
                var reader = command.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        string productName = reader["ProductName"].ToString();
                        decimal taxPercent = decimal.Parse((reader["Tax"]).ToString().Replace("%", "")) / 100;

                        productTaxes.Add(productName, taxPercent);
                    }
                }
            }

            return productTaxes;
        }

        public static void GenerateExcelFile(DateTime date)
        {
            const string ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=..\\..\\..\\..\\WritingFiles\\Products-Total-Report.xlsx;" +
                "Extended Properties=\'Excel 12.0 Xml;HDR=Yes;\'";

            OleDbConnection connection = new OleDbConnection(ConnectionString);
            connection.Open();
            using (connection)
            {
                // for excel writing direclty
                var expenses = ReadMongoDB.Generate(date);
                var incomes = ReadXML.Generate(date, date.AddMonths(1));
                var vendorAndProducts = TaxGenerator.GetVendorsAndProducts(date);
                var productTaxes = TaxGenerator.GetProductTaxes();

                decimal currTaxes = 0m;
                decimal finacialResult = 0m;

                var cmd = connection.CreateCommand();

                cmd.CommandText = "CREATE TABLE Report (Vendor char(255), Incomes double, Expenses double, Taxes double, [Financial Result] double)";
                cmd.ExecuteNonQuery();


                foreach (var vendorAndProduct in vendorAndProducts)
                {
                    foreach (var list in vendorAndProduct.Value)
                    {
                        currTaxes = productTaxes[list.Item1] * list.Item2 + currTaxes;
                    }

                    finacialResult = incomes[vendorAndProduct.Key] - expenses[vendorAndProduct.Key] - currTaxes;

                    string commandString =
                        "INSERT INTO [Report$] (Vendor, Incomes, Expenses, Taxes, [Financial Result])" +
                        "VALUES(@Vendor, @Incomes, @Expenses, @Taxes, @FinancialResult)";

                    OleDbCommand command = new OleDbCommand(commandString, connection);



                    command.Parameters.AddWithValue("@Vendor", vendorAndProduct.Key);
                    command.Parameters.AddWithValue("@Incomes", incomes[vendorAndProduct.Key]);
                    command.Parameters.AddWithValue("@Expenses", expenses[vendorAndProduct.Key]);
                    command.Parameters.AddWithValue("@Taxes", Math.Round(currTaxes,2));
                    command.Parameters.AddWithValue("@FinancialResult",Math.Round(finacialResult,2));

                    command.ExecuteNonQuery();

                    currTaxes = 0;
                    finacialResult = 0;
                }
            }
        }
    }
}
