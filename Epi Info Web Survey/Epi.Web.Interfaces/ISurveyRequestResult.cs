using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Epi.Web.Interfaces
{
    public interface ISurveyRequestResult
    {
        public bool IsPulished
        {
            get;
            set;
        }

        public string URL
        {
            get;
            set;
        }

        public string StatusText
        {
            get;
            set;
        }
    }
}
