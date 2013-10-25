using _01.CountCategories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class CountCategories
{
    static void Main()
    {
        SqlConnection dbCon = new SqlConnection(Settings.Default.dbConString);
        dbCon.Open();
        using (dbCon)
        {
            SqlCommand cmdCount = new SqlCommand(
                "SELECT COUNT(*) FROM Categories", dbCon);
            int categories = (int)cmdCount.ExecuteScalar();
            Console.WriteLine("Categories count: {0} ", categories);
        }
    }
}