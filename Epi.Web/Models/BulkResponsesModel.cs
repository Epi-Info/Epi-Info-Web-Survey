using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Epi.Web.MVC.Models
{
    public class BulkResponsesModel
    {
        public string Url { get; set; }

        public int Status { get; set; }

        public string DateCompleted { get; set; }
        public Dictionary<string, string> SurveyQuestionAnswerList { get; set; }
    }
}