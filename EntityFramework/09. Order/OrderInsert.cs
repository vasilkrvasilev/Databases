using EntityFramework.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

class OrderInsert
{
    static void Main()
    {
        TransactionScope scope = new TransactionScope();
        using (scope)
        {
            northwindEntities context = new northwindEntities();
            using (context)
            {
                Console.WriteLine("Enter Customer ID");
                string customer = Console.ReadLine();
                Console.WriteLine("Enter Employee ID");
                int employee = int.Parse(Console.ReadLine());
                Console.WriteLine("Enter Order Date in format dd.MM.yyyy");
                DateTime orderDate =
                    DateTime.ParseExact(Console.ReadLine(), "dd.MM.yyyy", new CultureInfo("en-US"));
                Console.WriteLine("Enter Required Date in format dd.MM.yyyy");
                DateTime requiredDate =
                    DateTime.ParseExact(Console.ReadLine(), "dd.MM.yyyy", new CultureInfo("en-US"));
                Console.WriteLine("Enter Shipped Date in format dd.MM.yyyy");
                DateTime shippedDate =
                    DateTime.ParseExact(Console.ReadLine(), "dd.MM.yyyy", new CultureInfo("en-US"));
                Console.WriteLine("Enter Sip Via");
                int via = int.Parse(Console.ReadLine());
                Console.WriteLine("Enter Freight");
                decimal freight = decimal.Parse(Console.ReadLine());
                Console.WriteLine("Enter Ship Name");
                string name = Console.ReadLine();
                Console.WriteLine("Enter Ship Address");
                string address = Console.ReadLine();
                Console.WriteLine("Enter Ship City");
                string city = Console.ReadLine();
                Console.WriteLine("Enter Ship Region");
                string region = Console.ReadLine();
                Console.WriteLine("Enter Ship Postal Code");
                string code = Console.ReadLine();
                Console.WriteLine("Enter Ship Country");
                string country = Console.ReadLine();
                Order order = new Order
                {
                    CustomerID = customer,
                    EmployeeID = employee,
                    OrderDate = orderDate,
                    RequiredDate = requiredDate,
                    ShippedDate = shippedDate,
                    ShipVia = via,
                    Freight = freight,
                    ShipName = name,
                    ShipAddress = address,
                    ShipCity = city,
                    ShipRegion = region,
                    ShipPostalCode = code,
                    ShipCountry = country
                };

                context.Orders.Add(order);
                context.SaveChanges();
            }

            scope.Complete();
        }
    }
}