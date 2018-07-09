using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Epi.Web.MVC.Models
{
    public class ResponseModel
    {
        public string Column0 { get; set; }
        public string Column1 { get; set; }
        public string Column2 { get; set; }
        public string Column3 { get; set; }
        public string Column4 { get; set; }
        public string Column5 { get; set; }

        public bool IsLocked { get; set; }

        public int PageSize { get; set; }
    }
}