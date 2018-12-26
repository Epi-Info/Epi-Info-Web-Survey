using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Epi.Web.MVC.Models
{
    public class SurveyResponseApiModel
    {
        public Guid SurveyId
        {
            get; set;
        }

        public Guid OrgKey
        {
            get; set;
        }

        public Guid PublisherKey
        {
            get; set;
        }

        public System.Collections.Generic.Dictionary<string, string> SurveyQuestionAnswerListField
        {
            get; set;
        }
    }
}