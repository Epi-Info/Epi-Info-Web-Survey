using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epi.Web.MVC.Models
{
    public class JsonMessage
    {
        public string ResponseId;
        public string FormId;
        public string FormName;
        public string ParentResponseId;
        public string ParentFormId;
        public string OKey;
        public object ResponseQA;
    }
}
