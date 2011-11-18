using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Epi.Web.Interfaces.BusinessInterface
{
    
    
    interface ISurveyBLL
    {
        Epi.Web.Interfaces.ISurveyRequestResult PublishSurvey(Epi.Web.Interfaces.ISurveyRequest pRequestMessage);
        Epi.Web.Interfaces.ISurveyRequest GetPublishedSurveyById(Guid gid);
    }
}
