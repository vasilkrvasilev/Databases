using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace SQLite_Mongo_Bridge
{
    public static class SQLiteWriter
    {
        public static void PopulateProductReportsInSQLite()
        {
            var reports = MongoReader.GetProductReports();
            const string connectionString = "Data Source=..\\..\\..\\..\\SQLiteDatabase\\SupermarketFinance.db";
            SQLiteConnection sqliteConnection = new SQLiteConnection(connectionString);
            sqliteConnection.Open();
            using (sqliteConnection)
            {
                string commandText =
                    "INSERT INTO ProductReports (ProductName, VendorName, TotalQuantitySold, TotalIncomes) " +
                    "VALUES (@productName, @vendorName, @totalquantitySold, @totalIncomes)";

                foreach (var report in reports)
                {
                    SQLiteCommand command = new SQLiteCommand(commandText, sqliteConnection);
                    command.Parameters.AddWithValue("@productName", report.ProductName);
                    command.Parameters.AddWithValue("@vendorName", report.VendorName);
                    command.Parameters.AddWithValue("@totalquantitySold", report.TotalQuantitySold);
                    command.Parameters.AddWithValue("@totalIncomes", report.TotalIncomes);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
