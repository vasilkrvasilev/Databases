using System;
using System.Linq;
using System.Data.OleDb;
using System.IO;
using System.IO.Compression;
using System.Globalization;
using System.Data.SqlClient;

namespace Document_IO_Operations
{
    public static class ReadExcel
    {
        public static void Execute()
        {
            ZipFile.ExtractToDirectory(Settings.Default.zipString, Settings.Default.extractString);
            SqlConnection dbCon = new SqlConnection(Settings.Default.sqlConnection);
            dbCon.Open();
            using (dbCon)
            {
                TraverseDFS(new DirectoryInfo(Settings.Default.extractString), dbCon);
            }
        }

        private static void TraverseDFS(DirectoryInfo directory, SqlConnection dbCon)
        {
            FileInfo[] files = directory.GetFiles();
            foreach (var file in files)
            {
                OleDbConnection connection = new OleDbConnection(string.Format(Settings.Default.excelString, file.FullName));
                connection.Open();
                using (connection)
                {
                    string dateString = file.Name.Substring(file.Name.Length - 15, 11);
                    DateTime date = DateTime.ParseExact(dateString, "dd-MMM-yyyy", new CultureInfo("en-US"));
                    int locationId = 0;
                    OleDbCommand commandSelect = new OleDbCommand("SELECT * FROM [Sales$]", connection);
                    OleDbDataReader readerSelect = commandSelect.ExecuteReader();
                    using (readerSelect)
                    {
                        readerSelect.Read();
                        string location = (string)readerSelect[0];
                        SqlCommand cmdLocationId = new SqlCommand(
                            "SELECT LocationID FROM Locations WHERE Supermarket=@name", dbCon);
                        cmdLocationId.Parameters.AddWithValue("@name", location);
                        SqlDataReader selectRreader = cmdLocationId.ExecuteReader();
                        using (selectRreader)
                        {
                            if (selectRreader.Read())
                            {
                                locationId = (int)selectRreader["LocationID"];
                            }
                        }
                        if (locationId == 0)
                        {
                            SqlCommand cmdInsertProject = new SqlCommand(
                                        "INSERT INTO Locations(Supermarket) " +
                                        "VALUES (@name)", dbCon);
                            cmdInsertProject.Parameters.AddWithValue("@name", location);
                            cmdInsertProject.ExecuteNonQuery();
                            SqlCommand cmdSelectIdentity = new SqlCommand("SELECT @@Identity", dbCon);
                            locationId = (int)(decimal)cmdSelectIdentity.ExecuteScalar();
                        }

                        readerSelect.Read();
                        while (readerSelect.Read())
                        {
                            if (((string)readerSelect[0]) == "…" || ((string)readerSelect[0]) == "Total sum:")
                            {
                                break;
                            }
                            int id = int.Parse(readerSelect[0].ToString());
                            double quantity = double.Parse(readerSelect[1].ToString());
                            decimal price = decimal.Parse(readerSelect[2].ToString());
                            double sum = double.Parse(readerSelect[3].ToString());
                            SqlCommand cmdInsertProject = new SqlCommand(
                                "INSERT INTO SalesReports(LocationID, Date, ProductID, Quantity, UnitPrice, Sum) " +
                                "VALUES (@location, @date, @product, @quantity, @price, @sum)", dbCon);
                            cmdInsertProject.Parameters.AddWithValue("@location", locationId);
                            cmdInsertProject.Parameters.AddWithValue("@date", date);
                            cmdInsertProject.Parameters.AddWithValue("@product", id);
                            cmdInsertProject.Parameters.AddWithValue("@quantity", quantity);
                            cmdInsertProject.Parameters.AddWithValue("@price", price);
                            cmdInsertProject.Parameters.AddWithValue("@sum", sum);
                            cmdInsertProject.ExecuteNonQuery();
                        }
                    }
                }
            }

            DirectoryInfo[] children = directory.GetDirectories();
            foreach (DirectoryInfo child in children)
            {
                TraverseDFS(child, dbCon);
            }
        }
    }
}