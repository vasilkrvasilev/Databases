using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Document_IO_Operations
{
    public class VendorExpenseEntity
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonRequired]
        public string VendorName { get; set; }

        [BsonRequired]
        public DateTime MonthDates { get; set; }

        [BsonRequired]
        public decimal Expenses { get; set; }
    }
}
