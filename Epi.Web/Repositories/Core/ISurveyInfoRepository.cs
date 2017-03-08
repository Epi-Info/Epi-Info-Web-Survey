using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Common.Message;

namespace Epi.Web.MVC.Repositories.Core
{
    /// <summary>
    /// SurveyInfo Repository interface.
    /// Derives from standard IRepository of SurveyInfoResponse. Adds a method GetSurveyInfo .
    /// </summary>
    public interface ISurveyInfoRepository: IRepository<Epi.Web.Common.Message.SurveyInfoResponse>
    {
        SurveyInfoResponse GetSurveyInfo(SurveyInfoRequest pRequestId);

        SurveyControlsResponse GetSurveyControlList(SurveyControlsRequest pRequestMessage);

        SurveyInfoResponse PublishExcelSurvey(SurveyInfoRequest pRequest);

        bool ValidateOrganization(OrganizationRequest Request);

        SurveyInfoResponse GetAllSurveysByOrgKey(string OrgKey);
        SourceTablesResponse GetSourceTables(SourceTablesRequest Request);
    }
}
