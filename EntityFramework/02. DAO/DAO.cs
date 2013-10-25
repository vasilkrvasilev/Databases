using EntityFramework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class DAO
{
    static void Main()
    {
        northwindEntities context = new northwindEntities();
        InsertCustomer(context);
        UpdateCustomer(context);
        DeleteCustomer(context);
    }

    static void InsertCustomer(northwindEntities context)
    {
        using (context)
        {
            Console.WriteLine("Enter Company name");
            string company = Console.ReadLine();
            Console.WriteLine("Enter Contact name");
            string contact = Console.ReadLine();
            Console.WriteLine("Enter Contact title");
            string title = Console.ReadLine();
            Console.WriteLine("Enter Address");
            string address = Console.ReadLine();
            Console.WriteLine("Enter City");
            string city = Console.ReadLine();
            Console.WriteLine("Enter Region");
            string region = Console.ReadLine();
            Console.WriteLine("Enter PostalCode");
            string code = Console.ReadLine();
            Console.WriteLine("Enter Country");
            string country = Console.ReadLine();
            Console.WriteLine("Enter Phone");
            string phone = Console.ReadLine();
            Console.WriteLine("Enter Fax");
            string fax = Console.ReadLine();
            Customer customer = new Customer
            {
                CompanyName = company,
                ContactName = contact,
                ContactTitle = title,
                Address = address,
                City = city,
                Region = region,
                PostalCode = code,
                Country = country,
                Phone = phone,
                Fax = fax,
            };
            context.Customers.Add(customer);
            context.SaveChanges();
        }
    }

    static void UpdateCustomer(northwindEntities context)
    {
        Console.WriteLine("Enter Customer ID");
        string id = Console.ReadLine();
        Customer customer = context.Customers.First(x => x.CustomerID == id);
        Console.WriteLine("Enter Column name");
        string column = Console.ReadLine();
        Console.WriteLine("Enter Column value");
        string value = Console.ReadLine();
        switch (column)
        {
            case "CompanyName": 
                customer.CompanyName = value;
                break;
            case "ContactName": 
                customer.ContactName = value;
                break;
            case "ContactTitle": 
                customer.ContactTitle = value;
                break;
            case "Address": 
                customer.Address = value;
                break;
            case "City": 
                customer.City = value;
                break;
            case "Region": 
                customer.Region = value;
                break;
            case "PostalCode": 
                customer.PostalCode = value;
                break;
            case "Country": 
                customer.Country = value;
                break;
            case "Phone": 
                customer.Phone = value;
                break;
            case "Fax": 
                customer.Fax = value;
                break;
        }

        context.SaveChanges();
    }

    static void DeleteCustomer(northwindEntities context)
    {
        Console.WriteLine("Enter Customer ID");
        string id = Console.ReadLine();
        Customer customer = context.Customers.First(x => x.CustomerID == id);
        context.Customers.Remove(customer);
        context.SaveChanges();
    }
}