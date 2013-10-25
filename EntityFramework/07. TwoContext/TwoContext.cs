using EntityFramework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class TwoContext
{
    static void Main()
    {
        using (northwindEntities firstContext = new northwindEntities())
        {
            using (northwindEntities secondContext = new northwindEntities())
            {
                Customer customerFirstContext = firstContext.Customers.Find("CHOPS");
                customerFirstContext.Region = "SW";

                Customer customerSecondContext = secondContext.Customers.Find("CHOPS");
                customerSecondContext.Region = "SSWW";

                firstContext.SaveChanges();
                secondContext.SaveChanges();
            }
        }

        Console.WriteLine("Changes successfully made!");
    }
}