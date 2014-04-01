using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Epi.Web.MVC.Models
    {
    public class PrintResponseModel
        {
        public List<PrintModel> ResponseList{get; set;}
        public int NumberOfPages{get; set;}

        public string SurveyName { get; set; }
        public string CurrentDate { get; set; }
        }
    }