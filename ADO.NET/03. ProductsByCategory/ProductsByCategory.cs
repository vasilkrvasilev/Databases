using _03.ProductsByCategory;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class ProductsByCategory
{
    static void Main()
    {
        SqlConnection dbCon = new SqlConnection(Settings.Default.dbConString);
        dbCon.Open();
        using (dbCon)
        {
            SqlCommand cmdSelect = new SqlCommand(
                "SELECT p.ProductName, c.CategoryName FROM Products p " +
                "JOIN Categories c ON p.CategoryID = c.CategoryID " +
                "GROUP BY CategoryName, ProductName", dbCon);

            SqlDataReader reader = cmdSelect.ExecuteReader();
            StringBuilder result = new StringBuilder();
            using (reader)
            {
                reader.Read();
                string categoryName = (string)reader["CategoryName"];
                string productName = (string)reader["ProductName"];
                Console.WriteLine(categoryName.Trim() + ":");
                result.Append(productName.Trim());
                while (reader.Read())
                {
                    string currentCategory = (string)reader["CategoryName"];
                    productName = (string)reader["ProductName"];
                    if (categoryName == currentCategory)
                    {
                        result.Append(", " + productName.Trim());
                    }
                    else
                    {
                        Console.WriteLine(result);
                        Console.WriteLine();
                        result = new StringBuilder();
                        categoryName = currentCategory;
                        Console.WriteLine(categoryName.Trim() + ":");
                        result.Append(productName.Trim());
                    }
                }
            }
        }
    }
}