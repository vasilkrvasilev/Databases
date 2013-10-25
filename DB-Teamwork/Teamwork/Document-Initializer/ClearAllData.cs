using System;
using System.IO;
using Supermarket.EF.Data;

namespace Document_Initializer
{
    public static class ClearAllData
    {
        public static void Execute()
        {
            string topPath = @"../../../../WritingFiles/";
            string jsonPath = @"../../../../WritingFiles/JSON/";

            Directory.Delete(topPath, true);

            Directory.CreateDirectory(topPath);
            Directory.CreateDirectory(jsonPath);



            SuperMarketEntities dbContext = new SuperMarketEntities();

            using (dbContext)
            {
                dbContext.Database.ExecuteSqlCommand("DELETE FROM SalesReports");
                dbContext.Database.ExecuteSqlCommand("DELETE FROM Locations");
                dbContext.Database.ExecuteSqlCommand("DELETE FROM Products");
                dbContext.Database.ExecuteSqlCommand("DELETE FROM VendorExpenses");
                dbContext.Database.ExecuteSqlCommand("DELETE FROM Vendors");
                dbContext.Database.ExecuteSqlCommand("DELETE FROM Measures");

                dbContext.Database.ExecuteSqlCommand("DBCC CHECKIDENT (SalesReports, reseed, 0)");
                dbContext.Database.ExecuteSqlCommand("DBCC CHECKIDENT (Locations, reseed, 0)");
                dbContext.Database.ExecuteSqlCommand("DBCC CHECKIDENT (Products, reseed, 0)");
                dbContext.Database.ExecuteSqlCommand("DBCC CHECKIDENT (VendorExpenses, reseed, 0)");
                dbContext.Database.ExecuteSqlCommand("DBCC CHECKIDENT (Vendors, reseed, 0)");
                dbContext.Database.ExecuteSqlCommand("DBCC CHECKIDENT (Measures, reseed, 0)");

                dbContext.SaveChanges();
            }
        }
    }
}
