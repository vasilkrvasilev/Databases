using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SupermarketEntiies;
using Supermarket.EF.Data;
using System.Data;

namespace OpenAccess_EF_Bridge
{
    public static class DataSender
    {
        private static SuperMarketEntities dbContext = null;

        private static List<ProductEntity> products = null;

        private static List<VendorEntity> vendors = null;

        private static List<MeasureEntity> measures = null;

        static DataSender()
        {
            dbContext = new SuperMarketEntities();

            products = new List<ProductEntity>();
            vendors = new List<VendorEntity>();
            measures = new List<MeasureEntity>();
        }

        public static void GetProducts(List<ProductEntity> products)
        {
            DataSender.products = products;
        }

        public static void GetVendors(List<VendorEntity> vendors)
        {
            DataSender.vendors = vendors;
        }

        public static void GetMeasures(List<MeasureEntity> measures)
        {
            DataSender.measures = measures;
        }

        public static void PopulateProducts()
        {
            Product currProduct = null;

            using (DataSender.dbContext = new SuperMarketEntities())
            {
                foreach (var product in DataSender.products)
                {
                    currProduct = new Product();

                    currProduct.ProductID = product.ProductID;
                    currProduct.ProductName = product.Name;
                    currProduct.MeasureID = product.MeasureID;
                    currProduct.BasePrice = product.BasePrice;
                    currProduct.VendorID = product.VendorID;


                    dbContext.Products.Add(currProduct);
                    dbContext.SaveChanges();
                }
            }
        }

        public static void PopulateVendors()
        {
            Vendor currVendor = null;

            using (DataSender.dbContext = new SuperMarketEntities())
            {
                foreach (var vendor in DataSender.vendors)
                {
                    currVendor = new Vendor();

                    currVendor.VendorID = vendor.VendorID;
                    currVendor.VendorName = vendor.Name;

                    dbContext.Vendors.Add(currVendor);
                    dbContext.SaveChanges();
                }
            }
        }

        public static void PopulateMeasures()
        {
            Measure currMeasure = null;

            using (DataSender.dbContext = new SuperMarketEntities())
            {
                foreach (var measure in DataSender.measures)
                {
                    currMeasure = new Measure();

                    currMeasure.MeasureID = measure.MeasureID;
                    currMeasure.Name = measure.Name;

                    dbContext.Measures.Add(currMeasure);
                    dbContext.SaveChanges();
                }
            }
        }
    }
}
