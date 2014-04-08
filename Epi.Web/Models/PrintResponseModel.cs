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
        public string ResponseId { get; set; }
        public string SurveyId { get; set; }
        public bool IsFromFinal { get; set; }
        }
    }