using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Document_IO_Operations
{
    public static class ReadXML
    {
        public static Dictionary<string, decimal> Generate(DateTime start, DateTime end)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("../../../../WritingFiles/Sales-by-Vendors-report.xml");
            XmlNode rootNode = doc.DocumentElement;
            var vendors = rootNode.SelectNodes("/sales/sale");
            Dictionary<string, decimal> incomes = new Dictionary<string, decimal>();
            for (int index = 0; index < vendors.Count; index++)
            {
                var name = vendors.Item(index).Attributes.Item(0).Value;
                decimal income = 0m;
                var summaries = vendors.Item(index).SelectNodes("summary");
                for (int innerIndex = 0; innerIndex < summaries.Count; innerIndex++)
                {
                    var itemDate = summaries.Item(innerIndex).Attributes.Item(0).Value;
                    DateTime itemRealDate = DateTime.ParseExact(itemDate, "dd-MMM-yyyy", new CultureInfo("en-US"));
                    if (itemRealDate >= start && itemRealDate <= end)
                    {
                        income += decimal.Parse(summaries.Item(innerIndex).Attributes.Item(1).Value);
                    }
                }

                incomes.Add(name, income);
            }

            return incomes;
        }
    }
}
