using EntityFramework.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Twin
{
    static void Main()
    {
        northwindEntities context = new northwindEntities();
        using (context)
        {
            CreatingNewDataBase(context);
        }
    }

    private static void CreatingNewDataBase(northwindEntities context)
    {
        SqlConnection myConn = new SqlConnection("Server=localhost;Integrated security=SSPI;database=master");
        string nativeSQLQuery = "CREATE DATABASE MyDatabase ON PRIMARY " +
        "(NAME = MyDatabase_Data, " +
        "FILENAME = 'C:\\MyDatabaseData.mdf', " +
        "SIZE = 2MB, MAXSIZE = 10MB, FILEGROWTH = 10%) " +
        "LOG ON (NAME = MyDatabase_Log, " +
        "FILENAME = 'C:\\MyDatabaseLog.ldf', " +
        "SIZE = 1MB, " +
        "MAXSIZE = 5MB, " +
        "FILEGROWTH = 10%)";
        context.Database.SqlQuery<string>(nativeSQLQuery);

        //SqlCommand myCommand = new SqlCommand(nativeSQLQuery, myConn);
        //try
        //{
        //    myConn.Open();
        //    myCommand.ExecuteNonQuery();    
        //}
        //finally
        //{
        //    if (myConn.State == ConnectionState.Open)
        //    {
        //        myConn.Close();
        //    }
        //}
    }
}