using _08.Products;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Products
{
    static void Main()
    {
        Console.WriteLine("Enter Product name");
        string name = Console.ReadLine();
        name = EscapeSQLString(name);
        SqlConnection dbCon = new SqlConnection(Settings.Default.dbConString);
        dbCon.Open();
        using (dbCon)
        {
            SqlCommand cmdSelect = new SqlCommand(
                "SELECT ProductName FROM Products WHERE ProductName LIKE @name ESCAPE '|'", dbCon);
            cmdSelect.Parameters.AddWithValue("@name", "%" + name + "%");

            Console.WriteLine("Products:");
            SqlDataReader reader = cmdSelect.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {
                    string productName = (string)reader["ProductName"];
                    Console.WriteLine(productName.Trim());
                }
            }
        }
    }

    static string EscapeSQLString(string name)
    {
        name = name.Replace("%", "|%");
        name = name.Replace("_", "|_");
        name = name.Replace("\\", "|\\");
        name = name.Replace("\"", "|\"");
        return name.Replace("'", "''");
    }
}