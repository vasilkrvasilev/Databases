using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using Supermarket.EF.Data;
using Supermarket.EF.Data;
using SQL_Mongo_Bridge;
using MongoDB.Driver;


namespace Document_IO_Operations
{
    public static class XMLDataTransefer
    {
        private const string VendorExpnesesSourcePath = @"../../../../ReadingFiles/Vendor-Expenses.xml";

        static XMLDataTransefer()
        {
        }

        public static void PopulateVendorExpensesInMongoDB()
        {
            MongoClient client = new MongoClient(MongoSettings.Default.MongoConnectionString);
            MongoServer server = client.GetServer();
            MongoDatabase productReportsDb = server.GetDatabase("productReport");

            Dictionary<int, string> vendors = GetVendorsIdNamePairs();

            MongoCollection<ProductReportEntity> vendorExpensesCollection = productReportsDb.GetCollection<ProductReportEntity>("vendorExpenses");

            List<VendorExpense> vendorExpenses = XMLDataTransefer.GetVendorExpenses();

            foreach (var expense in vendorExpenses)
            {
                for (int i = 0; i < expense.MonthDates.Count; i++)
                {
                    VendorExpenseEntity entity = new VendorExpenseEntity();
                    entity.VendorName = vendors[expense.VendorID];

                    entity.MonthDates = expense.MonthDates[i];
                    entity.Expenses = expense.Expenses[i];

                    vendorExpensesCollection.Insert(entity);
                }
            }
        }

        public static void PopulateVendorExpensesInSQL()
        {
            SuperMarketEntities dbContext = new SuperMarketEntities();

            List<VendorExpense> vendorExpenses = XMLDataTransefer.GetVendorExpenses();

            using(dbContext)
            {
                VendorExpens currReport = null;

                foreach (var vendorExpnese in vendorExpenses)
                {
                    for (int itemIdex = 0; itemIdex < vendorExpnese.MonthDates.Count; itemIdex++)
                    {
                        currReport = new VendorExpens();
                        currReport.VendorID = vendorExpnese.VendorID;

                        currReport.MonthDate = vendorExpnese.MonthDates[itemIdex];
                        currReport.Expnenses = vendorExpnese.Expenses[itemIdex];

                        dbContext.VendorExpenses.Add(currReport);
                       
                    }
                }

                dbContext.SaveChanges();
            }
        }

        private static List<VendorExpense> GetVendorExpenses()
        {
            Dictionary<string, int> vendors = GetVendorsNameIdPairs();

            List<VendorExpense> result = new List<VendorExpense>();
            XmlDocument document = new XmlDocument();

            document.Load(VendorExpnesesSourcePath);

            XmlNode rootNode = document.DocumentElement;

            VendorExpense currExpense = null;

            foreach (XmlNode saleVendor in rootNode.ChildNodes)
            {
                currExpense = new VendorExpense();

                foreach (XmlAttribute atr in saleVendor.Attributes)
	            {
		             currExpense.VendorID = vendors[atr.Value];
	            }
               
                foreach (XmlNode saleExpense in saleVendor)
                {
                    foreach (XmlAttribute innerAtr in saleExpense.Attributes)
                    {
                        currExpense.MonthDates.Add(DateTime.Parse(innerAtr.Value));
                    }

                    currExpense.Expenses.Add(decimal.Parse(saleExpense.InnerText));
                }

                result.Add(currExpense);
            }

            return result;
        }

        private static Dictionary<int, string> GetVendorsIdNamePairs()
        {
            Dictionary<int, string> vendors = new Dictionary<int, string>();

            SuperMarketEntities dbcontext = new SuperMarketEntities();

            using (dbcontext)
            {
                foreach (var vendor in dbcontext.Vendors)
                {
                    vendors.Add(vendor.VendorID, vendor.VendorName);
                }
            }

            return vendors;
        }

        private static Dictionary<string, int> GetVendorsNameIdPairs()
        {
            Dictionary<string, int> vendors = new Dictionary<string, int>();

            SuperMarketEntities dbcontext = new SuperMarketEntities();

            using (dbcontext)
            {
                foreach (var vendor in dbcontext.Vendors)
                {
                    vendors.Add(vendor.VendorName, vendor.VendorID);
                }
            }

            return vendors;
        }
    }
}
