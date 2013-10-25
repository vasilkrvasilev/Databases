using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.Data;

class CreateContext
{
    static void Main()
    {
        northwindEntities context = new northwindEntities();
        using (context)
        {
            foreach (var territory in context.Territories)
            {
                Console.WriteLine(territory.TerritoryDescription);
            }
        }
    }
}