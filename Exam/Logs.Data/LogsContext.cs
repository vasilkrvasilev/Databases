using Logs.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logs.Data
{
    public class LogsContext : DbContext
    {
        public LogsContext()
            : base("Logs")
        {
        }

        public DbSet<SearchLog> SearchLogs { get; set; }
    }
}
