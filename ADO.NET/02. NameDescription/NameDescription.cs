using _02.NameDescription;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class NameDescription
{
    static void Main()
    {
        SqlConnection dbCon = new SqlConnection(Settings.Default.dbConString);
        dbCon.Open();
        using (dbCon)
        {
            SqlCommand cmdSelect = new SqlCommand(
                "SELECT CategoryName, Description FROM Categories", dbCon);

            Console.WriteLine("Category:\tDescription");
            SqlDataReader reader = cmdSelect.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {
                    string name = (string)reader["CategoryName"];
                    string description = (string)reader["Description"];
                    Console.WriteLine("{0}:\t{1}", name.Trim(), description.Trim());
                }
            }
        }
    }
}