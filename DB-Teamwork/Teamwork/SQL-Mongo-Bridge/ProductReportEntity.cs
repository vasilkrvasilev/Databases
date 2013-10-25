using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SQL_Mongo_Bridge
{
    public class ProductReportEntity
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonRequired]
        public int ProductId { get; set; }

        [BsonRequired]
        public string ProductName { get; set; }

        [BsonRequired]
        public string VendorName { get; set; }

        [BsonRequired]
        public int TotalQuantitySold { get; set; }

        [BsonRequired]
        public decimal TotalIncomes { get; set; }
    }
}
