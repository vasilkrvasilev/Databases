using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework.Data
{
    public partial class Employee
    {
        public EntitySet<Territory> TerritoryAccess
        {
            get
            {
                EntitySet<Territory> territory = new EntitySet<Territory>(); 
                northwindEntities context = new northwindEntities();
                using (context)
                {
                    var collection = context.Employees.Select(e => e.Territories);
                    foreach (var item in collection)
                    {
                        territory.Add((Territory)item);
                    }

                    return territory;
                }
            }
        }
    }
}
