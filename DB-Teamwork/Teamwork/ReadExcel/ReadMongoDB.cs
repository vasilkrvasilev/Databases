using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Document_IO_Operations
{
    public static class ReadMongoDB
    {
        public static Dictionary<string, decimal> Generate(DateTime date)
        {
            var client = new MongoClient("mongodb://192.168.193.25:27017/");
            var server = client.GetServer();
            var db = server.GetDatabase("productReport");
            var expenses = db.GetCollection("vendorExpenses");
            var vendors = (from c in expenses.AsQueryable<VendorExpenseEntity>() where c.MonthDates == date select c);
            
            Dictionary<string, decimal> vendorExpenses = new Dictionary<string, decimal>();
            foreach (var item in vendors)
            {
                vendorExpenses.Add(item.VendorName, item.Expenses);
            }

            return vendorExpenses;
            
        }
    }
}