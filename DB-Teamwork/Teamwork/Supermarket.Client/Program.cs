using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Xml.Linq;
using System.Globalization;
using System.Data.SqlClient;
using OpenAccess_EF_Bridge;
using Document_IO_Operations;
using SQL_Mongo_Bridge;
using Telerik.OpenAccess;
using SQLite_Mongo_Bridge;
using Document_Initializer;

namespace Supermarket.Client
{
    internal class Program
    {
        static void Main()
        {
            ClearAllData.Execute();

            DataTransferer.Execute();
            ReadExcel.Execute();
            MongoWriter.WriteToDatabase();
            SQLReader.GetProductReports();
            SQLReader.WriteToFileSystem();
            PDFGenerator.Generate();
            XMLSales.Generate();
            XMLDataTransefer.PopulateVendorExpensesInSQL();
            XMLDataTransefer.PopulateVendorExpensesInMongoDB();
            SQLiteWriter.PopulateProductReportsInSQLite();
            
            TaxGenerator.GenerateExcelFile(new DateTime(2013,7,1));
        }
    }
}
