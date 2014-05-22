using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Epi.Web.Interfaces.DataInterface
{
    interface ISurveyDAL
    {
        Epi.Web.Interfaces.ISurveyRequestResult PublishSurvey(Epi.Web.Interfaces.ISurveyRequest pRequestMessage);
        Epi.Web.Interfaces.ISurveyRequest GetPublishedSurveyById(Guid gid);
    }
}
