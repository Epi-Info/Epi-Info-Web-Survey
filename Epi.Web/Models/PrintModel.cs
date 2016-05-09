using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Epi.Web.MVC.Models
    {
    public class PrintModel
        {
      
        public string Question { get; set; }
        public string Value { get; set; }
        public int PageNumber { get; set; }
        public string ControlName { get; set; }
        public string ControlType { get; set; }
        }
    }