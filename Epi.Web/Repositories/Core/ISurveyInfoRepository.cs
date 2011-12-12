using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Common.Message;

namespace Epi.Web.Repositories.Core
{
    /// <summary>
    /// SurveyInfo Repository interface.
    /// Derives from standard IRepository. Adds one SurveyInfo specific member.
    /// </summary>
    public interface ISurveyInfoRepository: IRepository<Epi.Web.Common.DTO.SurveyInfoDTO>
    {
        SurveyInfoResponse GetSurveyInfo(SurveyInfoRequest pRequestId);
    }
}
