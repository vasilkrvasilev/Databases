using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Logs.Models
{
    public class SearchLog
    {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string QueryXml { get; set; }
    }
}
