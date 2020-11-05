using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epi.Web.Models
{
    public class SourceTableModel
    {
        public string TableName   { get; set; }
        public string TableXml { get; set; }
        public bool AllowUpdate { get; set; }
    }
}
