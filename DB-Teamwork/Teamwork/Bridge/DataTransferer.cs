using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAccess_EF_Bridge
{
    public static class DataTransferer
    {
        public static void Execute()
        {
            DataSender.GetProducts(DataRetriever.GetProducts());

            DataSender.GetVendors(DataRetriever.GetVendors());

            DataSender.GetMeasures(DataRetriever.GetMeasures());

            DataSender.PopulateVendors();
            DataSender.PopulateMeasures();
            DataSender.PopulateProducts();
        }
    }
}
