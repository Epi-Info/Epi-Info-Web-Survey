using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Common.Message;

namespace Epi.Web.Repositories.Core
{
    /// <summary>
    /// SurveyResponse Repository interface.
    /// Derives from standard IRepository of SurveyResponseResponse. Adds a method GetSurveyResponse .
    /// </summary>
    public interface ISurveyResponseRepository: IRepository<Epi.Web.Common.Message.SurveyResponseResponse>
    {
        SurveyResponseResponse GetSurveyResponse(SurveyResponseRequest pRequest);
        SurveyResponseResponse SaveSurveyResponse(SurveyResponseRequest pRequest);
    }
}
