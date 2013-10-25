using EntityFramework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class NativeFind
{
    static void Main()
    {
        northwindEntities context = new northwindEntities();
        using (context)
        {
            string nativeSQLQuery = 
                "SELECT DISTINCT c.CompanyName " +
                "FROM dbo.Customers c " +
                "JOIN dbo.Orders o " +
                "ON c.CustomerID = o.CustomerID " +
                "WHERE YEAR(o.OrderDate) = 1997 AND o.ShipCountry = 'Canada'";
            var companies = context.Database.SqlQuery<string>(nativeSQLQuery);

            foreach (var company in companies)
            {
                Console.WriteLine(company);
            }
        }
    }
}