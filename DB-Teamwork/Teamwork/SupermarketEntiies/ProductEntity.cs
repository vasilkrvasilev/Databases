using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SupermarketEntiies
{
    public class ProductEntity
    {
        public int ProductID { get; set; }

        public int VendorID { get; set; }

        public string Name { get; set; }

        public int MeasureID { get; set; }

        public decimal BasePrice { get; set; }
    }
}
