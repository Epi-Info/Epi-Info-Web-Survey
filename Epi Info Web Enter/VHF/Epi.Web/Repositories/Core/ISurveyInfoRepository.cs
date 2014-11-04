using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Enter.Common.Message;

namespace Epi.Web.MVC.Repositories.Core
{
    /// <summary>
    /// SurveyInfo Repository interface.
    /// Derives from standard IRepository of SurveyInfoResponse. Adds a method GetSurveyInfo .
    /// </summary>
    public interface ISurveyInfoRepository: IRepository<Epi.Web.Enter.Common.Message.SurveyInfoResponse>
    {
        SurveyInfoResponse GetSurveyInfo(SurveyInfoRequest pRequestId);

        FormsInfoResponse GetFormsInfoList(FormsInfoRequest pRequestId);

        SurveyAnswerResponse DeleteResponse(SurveyAnswerRequest SARequest);

        SurveyInfoResponse GetFormChildInfo(SurveyInfoRequest SurveyInfoRequest);
        FormsHierarchyResponse GetFormsHierarchy(FormsHierarchyRequest FormsHierarchyRequest);
        SurveyAnswerResponse GetResponseAncestor(SurveyAnswerRequest SARequest);
    }
}
