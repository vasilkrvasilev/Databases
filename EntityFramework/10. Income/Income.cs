using EntityFramework.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Income
{
    static void Main()
    {
        northwindEntities context = new northwindEntities();
        using (context)
        {
            var supplierIncome = findSupplierIncome(context);
            Console.WriteLine("Supplier {0}'s income is {1}", "ss", supplierIncome);
        }
    }

    static IEnumerable<int> findSupplierIncome(northwindEntities context)
    {
        Console.WriteLine("Enter Company Name");
        string name = Console.ReadLine();
        Console.WriteLine("Enter Start Date in format dd.MM.yyyy");
        DateTime startDate =
            DateTime.ParseExact(Console.ReadLine(), "dd.MM.yyyy", new CultureInfo("en-US"));
        Console.WriteLine("Enter End Date in format dd.MM.yyyy");
        DateTime endDate =
            DateTime.ParseExact(Console.ReadLine(), "dd.MM.yyyy", new CultureInfo("en-US"));
        string nativeSQLQuery = string.Format(
            "select sum(od.UnitPrice * Quantity) as SupplierIncome " +
            "from dbo.Suppliers as s " +
            "inner join dbo.Products as p " +
            "on p.SupplierID = s.SupplierID " +
            "inner join dbo.[Order Details] as od " +
            "on od.ProductID = p.ProductID " +
            "inner join dbo.Orders as o " +
            "on o.OrderID = od.OrderID " +
            "where s.CompanyName = {0} and o.OrderDate > {1} and o.OrderDate > {2}",
            name, startDate, endDate);
        var income = context.Database.SqlQuery<int>(nativeSQLQuery);
        return income;
    }
}