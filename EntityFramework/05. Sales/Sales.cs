using EntityFramework.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Sales
{
    static void Main()
    {
        northwindEntities context = new northwindEntities();
        using (context)
        {
            Console.WriteLine("Enter Region name");
            string region = Console.ReadLine();
            Console.WriteLine("Enter Start date in format dd.MM.yyyy");
            DateTime startDate = 
                DateTime.ParseExact(Console.ReadLine(), "dd.MM.yyyy", new CultureInfo("en-US"));
            Console.WriteLine("Enter End date in format dd.MM.yyyy");
            DateTime endDate = 
                DateTime.ParseExact(Console.ReadLine(), "dd.MM.yyyy", new CultureInfo("en-US"));
            var sales = context.Orders.Select(x => x).
                Where(x => x.ShipRegion == region &&
                    x.OrderDate > startDate && x.OrderDate < endDate);

            Console.WriteLine(sales.Count());
        }
    }
}