using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlSupermarket.Data;
using SupermarketEntiies;
using Telerik.OpenAccess;

namespace OpenAccess_EntityFramework_Bridge
{
    public static class DataRetriever
    {
        public static List<ProductEntity> GetProducts()
        {
            List<ProductEntity> products = new List<ProductEntity>();

            MySqlSupermarketContext context = new MySqlSupermarketContext();
            using (context)
            {
                foreach (var product in context.Products)
                {
                    ProductEntity productEntity = new ProductEntity();
                    productEntity.ProductID = product.ID;
                    productEntity.Name = product.ProductName;
                    productEntity.BasePrice = product.BasePrice;
                    productEntity.VendorID = product.VendorID;
                    productEntity.MeasureID = product.MeasureID;

                    products.Add(productEntity);
                }
            }

            return products;
        }

        public static List<VendorEntity> GetVendors()
        {
            List<VendorEntity> vendors = new List<VendorEntity>();

            MySqlSupermarketContext context = new MySqlSupermarketContext();
            using (context)
            {
                foreach (var vendor in context.Vendors)
                {
                    VendorEntity vendorEntity = new VendorEntity();
                    vendorEntity.VendorID = vendor.VendorID;
                    vendorEntity.Name = vendor.VendorName;

                    vendors.Add(vendorEntity);
                }
            }

            return vendors;
        }

        public static List<MeasureEntity> GetMeasures()
        {
            List<MeasureEntity> measures = new List<MeasureEntity>();

            MySqlSupermarketContext context = new MySqlSupermarketContext();
            using (context)
            {
                foreach (var measure in context.Measures)
                {
                    MeasureEntity measureEntity = new MeasureEntity();
                    measureEntity.MeasureID = measure.MeasureID;
                    measureEntity.Name = measure.Name;

                    measures.Add(measureEntity);
                }
            }

            return measures;
        }
    }
}
