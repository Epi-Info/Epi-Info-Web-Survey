using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Epi.Web.BLL
{
    public class cSurveyRequestResult : Epi.Web.Interfaces.ISurveyRequestResult
    {
        bool isPulished;
        string uRL;
        string statusText;

        public bool IsPulished { get { return this.isPulished; } set { this.isPulished = value; } }

        public string URL { get { return this.uRL; } set { this.uRL = value; } }

        public string StatusText { get { return this.statusText; } set { this.statusText = value; } }
    }
}
