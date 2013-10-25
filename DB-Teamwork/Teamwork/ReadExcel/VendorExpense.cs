using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Document_IO_Operations
{
    public class VendorExpense
    {
        public int VendorExpenseID { get; set; }

        public int VendorID { get; set; }

        public List<DateTime> MonthDates { get; set; }

        public List<decimal> Expenses { get; set; }

        public VendorExpense()
        {
            this.MonthDates = new List<DateTime>();

            this.Expenses = new List<decimal>();
        }
    }
}
