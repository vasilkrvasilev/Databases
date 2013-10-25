using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelerikAcademy.Data;

class ToList
{
    static void Main()
    {
        TelerikAcademyEntities context = new TelerikAcademyEntities();
        using (context)
        {
            var employees = context.Employees.Select(e => e).ToList();
            var addresses = employees.Select(e =>
                new 
                { 
                    Name = e.FirstName + " " + e.LastName, 
                    Addtess = e.Address 
                }).ToList();

            var towns = addresses.Select(a => 
                new 
                { 
                    Name = a.Name, 
                    Town = a.Addtess.Town.Name 
                }).ToList();

            var sofia = towns.Select(t => t).Where(t => t.Town == "Sofia").ToList();
            foreach (var item in sofia)
            {
                Console.WriteLine(item.Name);
            }

            var selectSofia = context.Employees.Join(context.Addresses,
                (e => e.AddressID), (a => a.AddressID), (e, a) =>
                    new
                    {
                        Name = e.FirstName + " " + e.LastName,
                        TownID = a.TownID
                    }).Join(context.Towns,
                    (x => x.TownID), (t => t.TownID), (x, t) =>
                        new
                        {
                            Name = x.Name,
                            Town = t.Name
                        }).Select(x => x).Where(x => x.Town == "Sofia");

            foreach (var item in selectSofia)
            {
                Console.WriteLine(item.Name);
            }
        }
    }
}