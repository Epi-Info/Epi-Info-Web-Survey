using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Epi.Web.Interfaces
{
    public interface ISurveyRequestResult
    {
        bool IsPulished { get; set; }
        string URL { get; set; }
        string StatusText { get; set; }
    }
}
