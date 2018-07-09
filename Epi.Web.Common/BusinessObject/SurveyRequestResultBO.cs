using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Epi.Web.Common.BusinessObject
{

    public class SurveyRequestResultBO  
    {
        bool isPulished;
        string uRL;
        string statusText;
        Dictionary<int, string> viewIdAndFormIdList;
        public bool IsPulished { get { return this.isPulished; } set { this.isPulished = value; } }

        public string URL { get { return this.uRL; } set { this.uRL = value; } }

        public string StatusText { get { return this.statusText; } set { this.statusText = value; } }
        public Dictionary<int, string> ViewIdAndFormIdList { get { return this.viewIdAndFormIdList; } set { this.viewIdAndFormIdList = value; } }
    }
}
