using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Epi.Web.MVC.Models
{
    public class ReportModel
    {
        public string DateCreated { get; set; }
        public string Reportid { get; set; }
        public string DateEdited { get; set; }
        public int Version { get; set; }
        public string ReportHtml { get; set; }
        public string DataSource { get; set; }
        public int RecordCount { get; set; }
        public string ReportURL { get; set; }
        public string ReportName { get; set; }
    }
}