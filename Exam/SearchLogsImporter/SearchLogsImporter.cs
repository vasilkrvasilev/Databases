using Logs.Data;
using Logs.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
//using Logs.Data.Migrations;

public class SearchLogsImporter
{
    public static void AddLog(DateTime date, string query)
    {
        Database.SetInitializer(new MigrateDatabaseToLatestVersion
            <LogsContext, Logs.Data.Migrations.Configuration>());

        LogsContext context = new LogsContext();
        using (context)
        {
            SearchLog log = new SearchLog();
            log.Date = date;
            log.QueryXml = query;
            context.SearchLogs.Add(log);
            context.SaveChanges();
        }
    }
}