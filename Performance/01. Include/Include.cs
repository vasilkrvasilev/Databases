using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelerikAcademy.Data;

class Include
{
    static void Main()
    {
        TelerikAcademyEntities context = new TelerikAcademyEntities();
        using (context)
        {
            var employees = context.Employees.Select(e => e);
            foreach (var employee in employees)
            {
                Console.WriteLine("Name: {0} - Deparment: {1}, Town: {2}", 
                    employee.FirstName + " " + employee.LastName,
                    employee.Department.Name, employee.Address.TownID);
            }

            foreach (var employee in
                context.Employees.Include("Department").Include("Address").Include("Address.Town"))
            {
                Console.WriteLine("Name: {0} - Deparment: {1}, Town: {2}",
                    employee.FirstName + " " + employee.LastName,
                    employee.Department.Name, employee.Address.Town.Name);
            }
        }
    }
}