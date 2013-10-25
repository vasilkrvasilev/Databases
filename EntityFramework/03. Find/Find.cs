using EntityFramework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Find
{
    static void Main()
    {
        northwindEntities context = new northwindEntities();
        using (context)
        {
            var orders = context.Customers.
                Join(context.Orders,
                (c => c.CustomerID), (o => o.CustomerID), (c, o) =>
                    new
                    {
                        Customer = c.CompanyName,
                        OrderDate = o.OrderDate,
                        Country = o.ShipCountry
                    }).
                    Select(x => x).
                    Where(x => x.OrderDate.Value.Year == 1997 && x.Country == "Canada");
            var companies = orders.Select(x => x.Customer).Distinct();

            foreach (var company in companies)
            {
                Console.WriteLine(company);
            }
        }
    }
}